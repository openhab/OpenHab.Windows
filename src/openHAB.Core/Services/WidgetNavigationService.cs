using System.Collections.Generic;
using System.Linq;
using openHAB.Core.Client.Models;

namespace openHAB.Core.Services
{
    /// <summary>
    /// Service that keeps track of navigation between linkedpages.
    /// </summary>
    public static class WidgetNavigationService
    {
        private static readonly Stack<OpenHABWidget> WidgetBackStack = new Stack<OpenHABWidget>();
        private static OpenHABWidget _currentWidget;

        /// <summary>
        /// Gets a value indicating whether there is a previous widget on the backstack.
        /// </summary>
        public static bool CanGoBack => _currentWidget != null;

        /// <summary>
        /// Navigates the backstack to the passed in target.
        /// </summary>
        /// <param name="target">The openHAB widget to navigate to.</param>
        public static void Navigate(OpenHABWidget target)
        {
            if (target == _currentWidget)
            {
                return;
            }

            WidgetBackStack.Push(target);
            _currentWidget = target;
        }

        /// <summary>
        /// Go back to the previous openHAB widget.
        /// </summary>
        /// <returns>The previous visted widget.</returns>
        public static OpenHABWidget GoBack()
        {
            if (WidgetBackStack.Count == 0)
            {
                return null;
            }

            WidgetBackStack.Pop();
            _currentWidget = WidgetBackStack.Count == 0 ? null : WidgetBackStack.Peek();

            return _currentWidget;
        }

        /// <summary>
        /// Go back to the previous openHAB widget.
        /// </summary>
        /// <returns>The previous visted widget.</returns>
        public static OpenHABWidget GoBackToRoot()
        {
            OpenHABWidget widget = GoBack();
            while (widget != null && widget.Parent != null)
            {
                widget = GoBack();
            }

            _currentWidget = WidgetBackStack.Count == 0 ? null : WidgetBackStack.Peek();

            return _currentWidget;
        }

        /// <summary>
        /// Clears and resets the widget navigation.
        /// </summary>
        public static void ClearWidgetNavigation()
        {
            _currentWidget = null;
            WidgetBackStack.Clear();
        }

        /// <summary>Gets the navigated widgets.</summary>
        /// <value>The widgets.</value>
        public static List<OpenHABWidget> Widgets => WidgetBackStack.Reverse().ToList();
    }
}
