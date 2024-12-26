using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using openHAB.Windows.ViewModel;

namespace openHAB.Windows.View;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class LogViewerPage : Page
{
    /// <summary>Initializes a new instance of the <see cref="LogViewerPage" /> class.</summary>
    public LogViewerPage()
    {
        DataContext = Program.Host.Services.GetRequiredService<LogsViewModel>();

        this.InitializeComponent();
    }

    private void TextBlock_SizeChanged(object sender, Microsoft.UI.Xaml.SizeChangedEventArgs e)
    {
        LogScroller.ScrollToVerticalOffset(LogScroller.ScrollableHeight);
    }
}
