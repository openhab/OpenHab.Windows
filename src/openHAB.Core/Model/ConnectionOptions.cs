using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using openHAB.Core.Client.Connection.Contracts;
using openHAB.Core.Client.Connection.Models;

namespace openHAB.Core.Model;

/// <summary>
/// Represents the connection options for the OpenHAB application.
/// </summary>
public class ConnectionOptions
{
    private static readonly List<IConnectionProfile> _connectionProfiles = Client.Connection.Models.ConnectionProfiles.GetProfiles();

    /// <summary>
    /// Initializes a new instance of the <see cref="ConnectionOptions"/> class.
    /// </summary>
    public ConnectionOptions()
    {
        IsRunningInDemoMode = false;
    }

    /// <summary>
    /// Gets the list of available connection profiles.
    /// </summary>
    /// <value>The connection profiles.</value>
    public static List<IConnectionProfile> ConnectionProfiles => _connectionProfiles;

    /// <summary>
    /// Gets or sets a value indicating whether the application is currently running in demo mode.
    /// </summary>
    /// <value><c>true</c> if the application is running in demo mode; otherwise, <c>false</c>.</value>
    public bool? IsRunningInDemoMode
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the configuration to the OpenHAB local instance.
    /// </summary>
    /// <value>The local connection configuration.</value>
    public Connection LocalConnection
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the configuration to the OpenHAB remote instance.
    /// </summary>
    /// <value>The remote connection configuration.</value>
    public Connection RemoteConnection
    {
        get;
        set;
    }

    /// <summary>
    /// Saves the current connection options to a file.
    /// </summary>
    /// <returns><c>true</c> if the save operation was successful; otherwise, <c>false</c>.</returns>
    public bool Save()
    {
        try
        {
            string connectionSettings = JsonSerializer.Serialize(this);
            File.WriteAllText(AppPaths.ConnectionFilePath, connectionSettings, Encoding.UTF8);

            return true;
        }
        catch (System.Exception)
        {
            return false;
        }
    }
}
