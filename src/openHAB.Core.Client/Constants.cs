namespace openHAB.Core.Client;

/// <summary>
/// A class that holds all the constants for the app.
/// </summary>
public sealed class Constants
{
    /// <summary>
    /// Holds the constants used in server calls.
    /// </summary>
    public sealed class API
    {
        /// <summary>
        /// Url used for Demo mode.
        /// </summary>
        public const string DemoModeUrl = "https://demo.openhab.org/";

        /// <summary>
        /// Username for demo system.
        /// </summary>
        public const string DemoModeUser = "demo";

        /// <summary>
        /// User password for demo system.
        /// </summary>
        public const string DemoModeUserPwd = "demo";

        /// <summary>
        /// The call to get information about OpenHAB instance.
        /// </summary>
        public const string ServerInformation = "rest/";

        /// <summary>
        /// The call to fetch the sitemaps.
        /// </summary>
        public const string Sitemaps = "rest/sitemaps";

        /// <summary>
        /// The call to get an event-stream.
        /// </summary>
        public const string Events = "rest/events";

        /// <summary>
        /// The call to get items.
        /// </summary>
        public const string Items = "rest/items";
    }
}
