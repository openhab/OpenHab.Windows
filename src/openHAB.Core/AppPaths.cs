using System;
using System.IO;

namespace openHAB.Core;

/// <summary>
/// Represents the paths used by the application.
/// </summary>
public class AppPaths
{
    private const string _applicationName = "openHAB";

    /// <summary>
    /// Gets the directory path for application data.
    /// </summary>
    public static string ApplicationDataDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _applicationName);

    /// <summary>
    /// Gets the directory path for icon cache.
    /// </summary>
    public static string IconCacheDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _applicationName, "icons");

    /// <summary>
    /// Gets the directory path for logs.
    /// </summary>
    public static string LogsDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _applicationName, "logs");

    /// <summary>
    /// Gets the file path for settings.
    /// </summary>
    public static string SettingsFilePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _applicationName, "settings.json");

    /// <summary>
    /// Gets the file path for connection settings.
    /// </summary>
    public static string ConnectionFilePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _applicationName, "connection.json");
}
