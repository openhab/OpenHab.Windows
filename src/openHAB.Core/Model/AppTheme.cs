using Microsoft.UI.Xaml;

namespace openHAB.Core.Model;

/// <summary>
/// Specifies the application theme.
/// </summary>
public enum AppTheme
{
    /// <summary>
    /// Light theme.
    /// </summary>
    Light = ApplicationTheme.Light,

    /// <summary>
    /// Dark theme.
    /// </summary>
    Dark = ApplicationTheme.Dark,

    /// <summary>
    /// System default theme.
    /// </summary>
    System = 2
}
