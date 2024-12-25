using System;

namespace openHAB.Core.Client.Connection.Models;

/// <summary>Specifies the current connection profile.</summary>
public class ConnectionProfile
{
    /// <summary>Gets or sets the connection name.</summary>
    /// <value>The name.</value>
    public string Name
    {
        get; set;
    }

    /// <summary>Gets or sets the URL.</summary>
    /// <value>The URL.</value>
    public Uri Url
    {
        get; set;
    }

    /// <summary>Gets or sets the type of the connection.</summary>
    /// <value>The type of the connection.</value>
    public ConnectionType ConnectionType
    {
        get; set;
    }
}
