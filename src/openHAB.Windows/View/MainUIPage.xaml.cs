using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using openHAB.Windows.ViewModel;

namespace openHAB.Windows.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainUIPage : Page
    {
        public MainUIPage()
        {
            InitializeComponent();
            ViewModel = Program.Host.Services.GetRequiredService<MainUIViewModel>();

            DataContext = ViewModel;
        }

        public MainUIViewModel ViewModel
        {
            get;
            set;
        }
    }
}
