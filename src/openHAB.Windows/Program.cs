using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;

namespace openHAB.Windows
{

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
                    var context = new DispatcherQueueSynchronizationContext(DispatcherQueue.GetForCurrentThread());
                    SynchronizationContext.SetSynchronizationContext(context);

                    HostApplicationBuilder hostApplicationBuild = new HostApplicationBuilder(args);
                    hostApplicationBuild.Services.AddOpenHABServices();
                    hostApplicationBuild.Services.AddOpenHABViewModels();
                    hostApplicationBuild.Services.AddViews();

                    _host = hostApplicationBuild.Build();

                   var app = _host.Services.GetRequiredService<App>();

                    // TODO: add application initialization stuff after `App` is created, e.g. custom unhandled exception handlers
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error in application start callback: {ex.Message}.");
                }
            });
        }
    }

#endif
}
