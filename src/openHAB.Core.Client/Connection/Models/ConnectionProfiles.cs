using openHAB.Core.Client.Connection.Contracts;
using System.Collections.Generic;

namespace openHAB.Core.Client.Connection.Models
{
    /// <summary>
    /// Manages connection profiles.
    /// </summary>
    public class ConnectionProfiles
    {
        private static Dictionary<int, IConnectionProfile> _profiles = new Dictionary<int, IConnectionProfile>()
            {
                { 1, new DefaultConnectionProfile() },
                { 2, new LocalConnectionProfile() },
                { 3, new RemoteConnectionProfile() },
                { 4, new CloudConnectionProfile() }
            };

        /// <summary>
        /// Gets the connection profile by the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the connection profile.</param>
        /// <returns>The connection profile associated with the specified identifier.</returns>
        public static IConnectionProfile GetProfile(int id)
        {
            return _profiles[id];
        }

        /// <summary>
        /// Gets all connection profiles.
        /// </summary>
        /// <returns>A list of all connection profiles.</returns>
        public static List<IConnectionProfile> GetProfiles()
        {
            return new List<IConnectionProfile>(_profiles.Values);
        }
    }
}
