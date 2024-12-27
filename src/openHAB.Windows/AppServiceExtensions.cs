using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Layouts;
using NLog.Targets;
using openHAB.Core;
using openHAB.Core.Client;
using openHAB.Core.Client.Connection;
using openHAB.Core.Client.Connection.Contracts;
using openHAB.Core.Client.Contracts;
using openHAB.Core.Client.Event.Contracts;
using openHAB.Core.Client.Extensions;
using openHAB.Core.Client.Options;
using openHAB.Core.Model;
using openHAB.Core.Notification;
using openHAB.Core.Notification.Contracts;
using openHAB.Core.Services;
using openHAB.Core.Services.Contracts;
using openHAB.Windows.ViewModel;
using JsonAttribute = NLog.Layouts.JsonAttribute;

namespace openHAB.Windows;

/// <summary>
/// Extension methods for configuring OpenHAB services.
/// </summary>
public static class AppServiceExtensions
{
    /// <summary>
    /// Adds OpenHAB services to the specified IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
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

        services.AddOpenHabHttpClients();
        services.AddSingleton<IOpenHABClient, OpenHABClient>();

        services.AddSingleton<IConnectionService, ConnectionService>();
        services.AddSingleton<IIconCaching, IconCaching>();
        services.AddSingleton<IAppManager, AppManager>();
        services.AddSingleton<IItemManager, ItemManager>();
        services.AddSingleton<INotificationManager, NotificationManager>();
        services.AddSingleton<IOpenHABEventParser, OpenHABEventParser>();
        services.AddSingleton<SitemapService>();
    }

    /// <summary>
    /// Adds OpenHAB view models to the specified IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to add view models to.</param>
    public static void AddOpenHABViewModels(this IServiceCollection services)
    {
        services.AddTransient<MainViewModel>();
        services.AddTransient<SettingsViewModel>();
        services.AddTransient<ConfigurationViewModel>();
        services.AddTransient<LogsViewModel>();
        services.AddTransient<MainUIViewModel>();
    }

    /// <summary>
    /// Adds views to the specified IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to add views to.</param>
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

    /// <summary>
    /// Adds configuration settings to the specified IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to add configuration settings to.</param>
    /// <param name="configuration">The ConfigurationManager containing the configuration settings.</param>
    public static void AddConfiguration(this IServiceCollection services, ConfigurationManager configuration)
    {
        IConfiguration config = configuration;
        services.Configure<SettingOptions>(config);
        services.Configure<ConnectionOptions>(config);
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
            FileName = "${var:LogPath}/${shortdate}.json",
            Layout = layout,
            MaxArchiveFiles = 3,
            ArchiveEvery = FileArchivePeriod.Day,
        };

        LoggingConfiguration configuration = new LoggingConfiguration();
        configuration.AddTarget(fileTarget);
        configuration.AddRuleForAllLevels(fileTarget);

        string logsFolderPath = AppPaths.LogsDirectory;
        configuration.Variables["LogPath"] = logsFolderPath;

        return configuration;
    }
}
