using openHAB.Core.Client.Connection.Contracts;
using System.Text.Json.Serialization;

namespace openHAB.Core.Client.Connection.Models
{
    /// <summary>
    /// Connection configuration for OpenHAB service or cloud instance.
    /// </summary>
    public class Connection
    {
        /// <summary>Gets or sets the connection profile.</summary>
        /// <value>The profile.</value>
        [JsonIgnore]
        public IConnectionProfile Profile
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the connection profile identifier.
        /// </summary>
        /// <value>The identifier of the connection profile.</value>
        public int ProfileId
        {
            get
            {
                return this.Profile.Id;
            }
            set
            {
                Profile = ConnectionProfiles.GetProfile(value);
            }
        }

        /// <summary>Gets or sets the type of the connection.</summary>
        /// <value>The type of the connection.</value>
        public HttpClientType Type
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the URL to the OpenHAB server.
        /// </summary>
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the username for the OpenHAB server connection.
        /// </summary>
        public string Username
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the password for the OpenHAB connection.
        /// </summary>
        public string Password
        {
            get;
            set;
        }

        /// <summary>
        ///  Gets or sets a value indicating whether the application will ignore the SSL certificate.
        /// </summary>
        public bool? WillIgnoreSSLCertificate
        {
            get;
            set;
        }

        /// <summary>
        ///  Gets or sets a value indicating whether the application will ignore the SSL hostname.
        /// </summary>
        public bool? WillIgnoreSSLHostname
        {
            get;
            set;
        }
    }
}
