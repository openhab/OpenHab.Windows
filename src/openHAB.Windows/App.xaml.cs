using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;
using openHAB.Core.Model;
using openHAB.Core.Notification.Contracts;
using openHAB.Core.Services.Contracts;

namespace openHAB.Windows;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
    private readonly ILogger<App> _logger;
    private readonly IAppManager _appManager;
    private readonly INotificationManager _notificationManager;
    private static Window _mainWindow;
    private readonly IOptions<SettingOptions> _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="App" /> class.
    /// </summary>
    /// <param name="appManager">The application manager.</param>
    /// <param name="notificationManage">The notification manager.</param>
    /// <param name="options">The application settings options.</param>
    /// <param name="logger">The logger instance.</param>
    public App(IAppManager appManager, INotificationManager notificationManage, IOptions<SettingOptions> options, ILogger<App> logger)
    {
        _options = options;
        _logger = logger;
        _appManager = appManager;
        _notificationManager = notificationManage;

        InitializeComponent();

        RequestedTheme = _options.Value.AppTheme.ConvertToApplicationTheme();
        UnhandledException += App_UnhandledException;
        DispatcherQueue = DispatcherQueue.GetForCurrentThread();
    }

    /// <summary>
    /// Gets the dispatcher queue for the current thread.
    /// </summary>
    public static DispatcherQueue DispatcherQueue
    {
        get; private set;
    }

    /// <summary>
    /// Gets the main window of the application.
    /// </summary>
    public static Window MainWindow
    {
        get => _mainWindow;
    }

    /// <summary>
    /// Invoked when the application is launched normally by the end user. Other entry points
    /// will be used such as when the application is launched to open a specific file.
    /// </summary>
    /// <param name="e">Details about the launch request and process.</param>
    protected override async void OnLaunched(LaunchActivatedEventArgs e)
    {
        _logger.LogInformation("=== Start Application ===");
        _appManager.SetProgramLanguage(null);

        // TODO This code defaults the app to a single instance app. If you need multi instance app, remove this part.
        // Read: https://docs.microsoft.com/en-us/windows/apps/windows-app-sdk/migrate-to-windows-app-sdk/guides/applifecycle#single-instancing-in-applicationonlaunched
        // If this is the first instance launched, then register it as the "main" instance.
        // If this isn't the first instance launched, then "main" will already be registered,
        // so retrieve it.
        AppInstance mainInstance = AppInstance.FindOrRegisterForKey("main");
        AppActivationArguments activatedEventArgs = AppInstance.GetCurrent().GetActivatedEventArgs();

        DispatcherQueue = DispatcherQueue.GetForCurrentThread();

        // Register for toast activation. Requires Microsoft.Toolkit.Uwp.Notifications NuGet package version 7.0 or greater
        ToastNotificationManagerCompat.OnActivated += ToastNotificationManagerCompat_OnActivated;

        // If the instance that's executing the OnLaunched handler right now
        // isn't the "main" instance.
        if (!mainInstance.IsCurrent)
        {
            // Redirect the activation (and args) to the "main" instance, and exit.
            await mainInstance.RedirectActivationToAsync(activatedEventArgs);
            System.Diagnostics.Process.GetCurrentProcess().Kill();
            return;
        }

        if (activatedEventArgs.Kind == ExtendedActivationKind.ToastNotification)
        {

        }

        // Initialize MainWindow here
        _mainWindow = Program.Host.Services.GetRequiredService<MainWindow>();

        _appManager.SetAppTheme(MainWindow.Content);

        MainWindow.Activate();
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        _logger.LogCritical(e.Exception, "Unhandled Exception");
    }

    private void ToastNotificationManagerCompat_OnActivated(ToastNotificationActivatedEventArgsCompat e)
    {
        // Use the dispatcher from the window if present, otherwise the app dispatcher
        var dispatcherQueue = MainWindow?.DispatcherQueue ?? App.DispatcherQueue;

        dispatcherQueue.TryEnqueue(delegate
        {
            var args = ToastArguments.Parse(e.Argument);

            switch (args["action"])
            {
                //// Send a background message
                //case "show":
                //    string message = e.UserInput["textBox"].ToString();
                //    // TODO: Send it

                //    // If the UI app isn't open
                //    if (MainWindow == null)
                //    {
                //        // Close since we're done
                //        Process.GetCurrentProcess().Kill();
                //    }

                //    break;

                // View a message
                case "show":

                    string itemName = args["item"];
                    // Launch/bring window to foreground
                    //LaunchAndBringToForegroundIfNeeded();

                    // TODO: Open the message
                    break;
            }
        });
    }
}
