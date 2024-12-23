using Microsoft.UI.Xaml.Shapes;
using Microsoft.Windows.ApplicationModel.Resources;

namespace openHAB.Common
{
    /// <summary>
    ///   Resources.
    /// </summary>
    public static class AppResources
    {
        private static ResourceLoader _resourceLoader;
        private static ResourceLoader _errorResourceLoader;

        /// <summary>
        ///   Gets the localized UI values.
        /// </summary>
        public static ResourceLoader Values
        {
            get
            {
                if (_resourceLoader == null)
                {
                    _resourceLoader = new ResourceLoader(ResourceLoader.GetDefaultResourceFilePath());
                }

                return _resourceLoader;
            }
        }

        /// <summary>
        ///   Gets the localized error strings.
        /// </summary>
        public static ResourceLoader Errors
        {
            get
            {
                if (_errorResourceLoader == null)
                {
                    _errorResourceLoader = new ResourceLoader(ResourceLoader.GetDefaultResourceFilePath());
                }

                return _errorResourceLoader;
            }
        }
    }
}
