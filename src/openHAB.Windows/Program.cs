using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using openHAB.Core.Services;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace openHAB.Windows;


#if DISABLE_XAML_GENERATED_MAIN
public static partial class Program
{
    private static IHost _host;

    public static IHost Host
    {
        get => _host;
        set => _host = value;
    }

    /// <summary>
    /// Ensures that the process can run XAML, and provides a deterministic
    /// error if a check fails. Otherwise, it quietly does nothing.
    /// </summary>
    [LibraryImport("Microsoft.ui.xaml.dll")]
    private static partial void XamlCheckProcessRequirements();

    [STAThread]
    private static void Main(string[] args)
    {
        // TODO: add application initialization stuff before XAML things happen

        // Taken from the default generated XAML entry point
        XamlCheckProcessRequirements();
        WinRT.ComWrappersSupport.InitializeComWrappers();
        Application.Start(_ =>
        {
            try
            {
                DispatcherQueueSynchronizationContext? context = new DispatcherQueueSynchronizationContext(DispatcherQueue.GetForCurrentThread());
                SynchronizationContext.SetSynchronizationContext(context);

                HostApplicationBuilder builder = new HostApplicationBuilder(args);
                builder.Configuration.AddJsonFile(AppPaths.SettingsFilePath, optional: true, reloadOnChange: true);

                builder.Services.AddConfiguration(builder.Configuration);
                builder.Services.AddOpenHABServices();
                builder.Services.AddOpenHABViewModels();
                builder.Services.AddViews();

                _host = builder.Build();

                App? app = _host.Services.GetRequiredService<App>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in application start callback: {ex.Message}.");
            }
        });
    }
}

#endif
