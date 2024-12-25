using openHAB.Core.Client.Connection.Contracts;
using openHAB.Core.Client.Connection.Models;
using System.Collections.Generic;

namespace openHAB.Core.Model;

/// <summary>
/// Class that holds all the OpenHAB Windows app settings.
/// </summary>
[System.Runtime.InteropServices.Guid("6AF3A86A-9AAA-400B-AB7F-E42A780D5ECF")]
public class Settings
{
    private static readonly List<IConnectionProfile> _connectionProfiles = Client.Connection.Models.ConnectionProfiles.GetProfiles();

    /// <summary>
    /// Initializes a new instance of the <see cref="Settings"/> class.
    /// </summary>
    public Settings()
    {
        IsRunningInDemoMode = false;
        ShowDefaultSitemap = false;
        UseSVGIcons = false;
        NotificationsEnable = false;
    }

    /// <summary>Gets the list of available connection profiles.</summary>
    /// <value>The connection profiles.</value>
    public static List<IConnectionProfile> ConnectionProfiles => _connectionProfiles;

    /// <summary>
    /// Gets or sets the application language.
    /// </summary>
    /// <value>The application language.</value>
    public string AppLanguage
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the application is currently running in demo mode.
    /// </summary>
    public bool? IsRunningInDemoMode
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the last sitemap name in settings.
    /// </summary>
    /// <value>
    /// The last sitemap.
    /// </value>
    public string LastSitemap
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets the configuration to the OpenHAB remote instance.
    /// </summary>
    public Connection LocalConnection
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the setting to enable notifications.
    /// </summary>
    /// <value>The enable notifications.</value>
    public bool? NotificationsEnable
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the configuration to the OpenHAB remote instance.
    /// </summary>
    public Connection RemoteConnection
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the default sitemap should be visible.
    /// </summary>
    /// <value>The hide default sitemap.</value>
    public bool ShowDefaultSitemap
    {
        get;
        set;
    }

    /// <summary>Gets or sets the setting to start application minimized.</summary>
    /// <value>The start application minimized.</value>
    public bool? StartAppMinimized
    {
        get;
        set;
    }

    /// <summary>Gets or sets a value indicating whether [use SVG icons].</summary>
    /// <value>
    ///   <c>true</c> if [use SVG icons]; otherwise, <c>false</c>.</value>
    public bool UseSVGIcons
    {
        get;
        set;
    }
}
