using Microsoft.UI.Xaml;
using openHAB.Common;
using openHAB.Core.Model;

namespace openHAB.Windows.ViewModel;

public class ApplicationThemeViewModel : ViewModelBase<AppTheme>
{
    private string _name;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationThemeViewModel"/> class.
    /// </summary>
    /// <param name="model">The application theme model.</param>
    public ApplicationThemeViewModel(AppTheme model)
        : base(model)
    {
        switch (Model)
        {
            case AppTheme.Light:
                Name = AppResources.Values.GetString("AppThemeLight");
                break;
            case AppTheme.Dark:
                Name = AppResources.Values.GetString("AppThemeDark");
                break;
            case AppTheme.System:
                Name = AppResources.Values.GetString("AppThemeSystem");
                break;
            default:
                Name = "Unknown";
                break;
        }
    }

    /// <summary>
    /// Gets the name of the application theme.
    /// </summary>
    public string Name
    {
        get => _name;
        set
        {
            Set(ref _name, value);
        }
    }

    /// <summary>
    /// Gets the identifier of the application theme.
    /// </summary>
    public int Id => (int)Model;

    /// <summary>
    /// Gets the application theme.
    /// </summary>
    public ApplicationTheme Theme => (ApplicationTheme)Model;
}
