using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI;
using Microsoft.Extensions.Logging;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using openHAB.Common;
using openHAB.Core.Client.Messages;
using openHAB.Core.Client.Models;
using openHAB.Core.Messages;
using openHAB.Windows.Services;
using openHAB.Windows.View;
using openHAB.Windows.ViewModel;
using Windows.ApplicationModel;

namespace openHAB.Windows
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private ILogger<MainPage> _logger;

        /// <summary>
        /// Gets the data context, for use in compiled bindings.
        /// </summary>
        public MainViewModel Vm
        {
            get;
            private set;
        }

        public Frame RootFrame => ContentFrame;

        public MainWindow()
        {
            _logger = DIService.Instance.GetService<ILogger<MainPage>>();

            this.InitializeComponent();
            this.ExtendsContentIntoTitleBar = true;
            this.AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;
            this.AppWindow.TitleBar.ButtonForegroundColor = Colors.Black;
            TitleBarTextBlock.Text = AppInfo.Current.DisplayInfo.DisplayName;

            Vm = DIService.Instance.GetService<MainViewModel>();
            Root.DataContext = Vm;

            StrongReferenceMessenger.Default.Register<ConnectionErrorMessage>(this, async (recipient, msg) => await ShowErrorMessage(recipient, msg));
            StrongReferenceMessenger.Default.Register<FireInfoMessage>(this, async (recipient, msg) => await ShowInfoMessage(recipient, msg));

            Vm.LoadSitemapsAndItemData();
        }

        private async Task ShowErrorMessage(object recipient, ConnectionErrorMessage message)
        {
            try
            {
                string errorMessage = null;
                if (message == null || string.IsNullOrEmpty(message.ErrorMessage))
                {
                    errorMessage = AppResources.Values.GetString("MessageError");
                    ErrorNotification.Message = errorMessage;
                    ErrorNotification.IsOpen = true;
                }
                else
                {
                    errorMessage = message.ErrorMessage;
                }

                await App.DispatcherQueue.EnqueueAsync(() =>
                {
                    ErrorNotification.Message = errorMessage;
                    ErrorNotification.IsOpen = true;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Show error message failed.");
            }
        }

        private async Task ShowInfoMessage(object recipient, FireInfoMessage msg)
        {
            try
            {
                string message = null;
                switch (msg.MessageType)
                {
                    case MessageType.NotConfigured:
                        message = AppResources.Values.GetString("MessageNotConfigured");
                        break;

                    case MessageType.NotReachable:
                        message = AppResources.Values.GetString("MessagesNotReachable");
                        break;

                    default:
                        message = "Message not defined";
                        break;
                }

                await App.DispatcherQueue.EnqueueAsync(() =>
                {
                    InfoNotification.Message = message;
                    InfoNotification.IsOpen = true;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Show info message failed.");
            }
        }

        private void SitemapNavigation_SelectionChanged(
            NavigationView sender,
            NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                sender.AlwaysShowHeader = false;
                ContentFrame.Navigate(typeof(SettingsPage));
            }
            else
            {
                sender.AlwaysShowHeader = true;
                OpenHABSitemap sitemap = args.SelectedItem as OpenHABSitemap;
                ContentFrame.Navigate(typeof(SitemapPage), sitemap.Link);
            }
        }

        private void BreadcrumbBar_ItemClicked(BreadcrumbBar sender, BreadcrumbBarItemClickedEventArgs args)
        {
            OpenHABWidget widget = args.Item as OpenHABWidget;
            StrongReferenceMessenger.Default.Send<WigetNavigation>(new WigetNavigation(null, widget, EventTriggerSource.Breadcrumb));

            //this.Vm.SelectedSitemap.WidgetGoBack(widget);
        }
    }
}
