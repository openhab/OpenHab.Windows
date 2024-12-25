using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Layouts;
using NLog.Targets;
using openHAB.Core.Client;
using openHAB.Core.Client.Connection;
using openHAB.Core.Client.Connection.Contracts;
using openHAB.Core.Client.Contracts;
using openHAB.Core.Client.Event.Contracts;
using openHAB.Core.Notification;
using openHAB.Core.Notification.Contracts;
using openHAB.Core.Services;
using openHAB.Core.Services.Contracts;
using openHAB.Windows.View;
using openHAB.Windows.ViewModel;
using JsonAttribute = NLog.Layouts.JsonAttribute;

namespace openHAB.Windows;

public static class AppServiceExtensions
{
    public static void AddOpenHABServices(this IServiceCollection services)
    {
        services.AddLogging(loggingBuilder =>
        {
            // configure Logging with NLog
            loggingBuilder.ClearProviders();
            loggingBuilder.SetMinimumLevel(LogLevel.Information);
            loggingBuilder.AddNLog(GetLoggingConfiguration());
        });

        services.AddSingleton<IMessenger>(StrongReferenceMessenger.Default);
        services.AddSingleton<IOpenHABClient, OpenHABClient>();
        services.AddSingleton<ISettingsService, SettingsService>();
        services.AddTransient(x =>
        {
            ISettingsService settingsService = x.GetService<ISettingsService>();
            return settingsService.Load();
        });

        services.AddSingleton<AppPaths>();
        services.AddSingleton<OpenHABHttpClient>();
        services.AddSingleton<IConnectionService, ConnectionService>();
        services.AddSingleton<IIconCaching, IconCaching>();
        services.AddSingleton<IAppManager, AppManager>();
        services.AddSingleton<IItemManager, ItemManager>();
        services.AddSingleton<INotificationManager, NotificationManager>();
        services.AddSingleton<IOpenHABEventParser, OpenHABEventParser>();
        services.AddSingleton<SitemapService>();
    }

    public static void AddOpenHABViewModels(this IServiceCollection services)
    {
        services.AddTransient<MainViewModel>();
        services.AddTransient<SettingsViewModel>();
        services.AddTransient<ConfigurationViewModel>();
        services.AddTransient<LogsViewModel>();
        services.AddTransient<MainUIViewModel>();
    }

    public static void AddViews(this IServiceCollection services)
    {
        services.AddSingleton<App>();
        services.AddSingleton<MainWindow>();

        //services.AddTransient<MainPage>();
        //services.AddTransient<MainUIPage>();
        //services.AddTransient<SettingsPage>();
        //services.AddTransient<LogViewerPage>();
        //services.AddTransient<SitemapPage>();
    }


    private static LoggingConfiguration GetLoggingConfiguration()
    {
        JsonLayout layout = new JsonLayout()
        {
            IncludeEventProperties = true,
        };

        layout.Attributes.Add(new JsonAttribute("time", @"${date:format=HH\:mm\:ss}"));
        layout.Attributes.Add(new JsonAttribute("level", "${level:upperCase=true}"));
        layout.Attributes.Add(new JsonAttribute("callsite", "${callsite:includeSourcePath=false}"));
        layout.Attributes.Add(new JsonAttribute("message", "${message}"));
        layout.Attributes.Add(new JsonAttribute("exception", "${exception:format=ToString}"));
        layout.Attributes.Add(new JsonAttribute("stacktrace", "${onexception:inner=${stacktrace:topFrames=10}}"));

        FileTarget fileTarget = new FileTarget("file")
        {
            FileName = "${var:LogPath}/logs/${shortdate}.json",
            Layout = layout,
            MaxArchiveFiles = 3,
            ArchiveEvery = FileArchivePeriod.Day,
        };

        LoggingConfiguration configuration = new LoggingConfiguration();
        configuration.AddTarget(fileTarget);
        configuration.AddRuleForAllLevels(fileTarget);

        string logsFolderPath = new AppPaths().LogsDirectory;
        configuration.Variables["LogPath"] = logsFolderPath;

        return configuration;
    }
}
