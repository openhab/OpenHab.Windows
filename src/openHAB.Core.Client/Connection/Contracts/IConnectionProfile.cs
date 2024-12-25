using openHAB.Core.Client.Connection.Models;

namespace openHAB.Core.Client.Connection.Contracts;

/// <summary>
/// Interface for connection profiles.
/// </summary>
public interface IConnectionProfile
{
    /// <summary>Gets a value indicating whether [host URL] value can be modified.</summary>
    /// <value>
    ///   <c>true</c> if [host URL configuration] can be modified; otherwise, <c>false</c>.</value>
    bool AllowHostUrlConfiguration
    {
        get;
    }

    /// <summary>Gets a value indicating whether [allow ignore SSL certificate] issues option is available.</summary>
    /// <value>
    ///   <c>true</c> if [allow ignore SSL certificate] is available; otherwise, <c>false</c>.</value>
    bool AllowIgnoreSSLCertificate
    {
        get;
    }

    /// <summary>Gets a value indicating whether [allow ignore SSL hostname] issue option is available.</summary>
    /// <value>
    /// <c>true.</c> if [allow ignore SSL hostname] can be enabled; otherwise, <c>false</c>.</value>
    bool AllowIgnoreSSLHostname
    {
        get;
    }

    /// <summary>Gets the connection profile id.</summary>
    /// <value>The id.</value>
    int Id
    {
        get;
    }

    /// <summary>Gets the connection profile name.</summary>
    /// <value>The profile name.</value>
    string Name
    {
        get;
    }

    /// <summary>Gets the profile type.</summary>
    /// <value>The type.</value>
    HttpClientType Type
    {
        get;
    }
    /// <summary>Gets the main UI URL.</summary>
    /// <value>The main UI URL.</value>
    string MainUIUrl
    {
        get;
    }
    /// <summary>Gets the URL.</summary>
    /// <value>The URL.</value>
    string Url
    {
        get;
    }

    /// <summary>Creates the connection based on the profile.</summary>
    /// <returns>Preconfigured connection.</returns>
    Models.Connection CreateConnection();
}
