﻿using System;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace OpenHAB.Windows.Controls
{
    /// <summary>
    /// A specialized ListView to represent the items in the navigation menu.
    /// </summary>
    /// <remarks>
    /// This class handles the following:
    /// 1. Sizes the panel that hosts the items so they fit in the hosting pane.  Otherwise, the keyboard
    ///    may appear cut off on one side b/c the Pane clips instead of affecting layout.
    /// 2. Provides a single selection experience where keyboard focus can move without changing selection.
    ///    Both the 'Space' and 'Enter' keys will trigger selection.  The up/down arrow keys can move
    ///    keyboard focus without triggering selection.  This is different than the default behavior when
    ///    SelectionMode == Single.  The default behavior for a ListView in single selection requires using
    ///    the Ctrl + arrow key to move keyboard focus without triggering selection.  Users won't expect
    ///    this type of keyboarding model on the nav menu.
    /// </remarks>
    public class NavMenuListView : ListView
    {
        private SplitView _splitViewHost;

        /// <summary>
        /// Event that fires whenever menu items are updated
        /// </summary>
        public event EventHandler MenuItemsUpdated;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavMenuListView"/> class.
        /// </summary>
        public NavMenuListView()
        {
            SelectionMode = ListViewSelectionMode.Single;
            IsItemClickEnabled = true;
            ItemClick += ItemClickedHandler;

            // Locate the hosting SplitView control
            Loaded += (s, a) =>
            {
                var parent = VisualTreeHelper.GetParent(this);
                while (parent != null && !(parent is SplitView))
                {
                    parent = VisualTreeHelper.GetParent(parent);
                }

                if (parent == null)
                {
                    return;
                }

                _splitViewHost = parent as SplitView;

                _splitViewHost.RegisterPropertyChangedCallback(SplitView.IsPaneOpenProperty, (sender, args) =>
                {
                    OnPaneToggled();
                });

                // Call once to ensure we're in the correct state
                OnPaneToggled();
            };
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Remove the entrance animation on the item containers.
            for (int i = 0; i < ItemContainerTransitions.Count; i++)
            {
                if (ItemContainerTransitions[i] is EntranceThemeTransition)
                {
                    ItemContainerTransitions.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Mark the <paramref name="item"/> as selected and ensures everything else is not.
        /// If the <paramref name="item"/> is null then everything is unselected.
        /// </summary>
        /// <param name="item">The selected menu item.</param>
        public void SetSelectedItem(ListViewItem item)
        {
            int index = -1;
            if (item != null)
            {
                index = IndexFromContainer(item);
            }

            if (Items == null)
            {
                return;
            }

            for (int i = 0; i < Items.Count; i++)
            {
                var lvi = (ListViewItem)ContainerFromIndex(i);

                if (lvi == null)
                {
                    continue;
                }

                if (i != index)
                {
                    lvi.IsSelected = false;
                }
                else if (i == index)
                {
                    lvi.IsSelected = true;
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnItemsChanged(object e)
        {
            base.OnItemsChanged(e);
            MenuItemsUpdated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Occurs when an item has been selected
        /// </summary>
        public event EventHandler<ListViewItem> ItemInvoked;

        /// <summary>
        /// Custom keyboarding logic to enable movement via the arrow keys without triggering selection
        /// until a 'Space' or 'Enter' key is pressed.
        /// </summary>
        /// <param name="e">Routed event args.</param>
        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            var focusedItem = FocusManager.GetFocusedElement();

            switch (e.Key)
            {
                case VirtualKey.Up:
                    TryMoveFocus(FocusNavigationDirection.Up);
                    e.Handled = true;
                    break;

                case VirtualKey.Down:
                    TryMoveFocus(FocusNavigationDirection.Down);
                    e.Handled = true;
                    break;

                case VirtualKey.Tab:
                    var shiftKeyState = CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Shift);
                    var shiftKeyDown = (shiftKeyState & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;

                    // If we're on the header item then this will be null and we'll still get the default behavior.
                    var item = focusedItem as ListViewItem;
                    if (item != null)
                    {
                        var currentItem = item;

                        if (Items == null)
                        {
                            return;
                        }

                        bool onlastitem = IndexFromContainer(currentItem) == Items.Count - 1;
                        bool onfirstitem = IndexFromContainer(currentItem) == 0;

                        if (!shiftKeyDown)
                        {
                            TryMoveFocus(onlastitem ? FocusNavigationDirection.Next : FocusNavigationDirection.Down);
                        }
                        else
                        {
                            // Shift + Tab
                            TryMoveFocus(onfirstitem ? FocusNavigationDirection.Previous : FocusNavigationDirection.Up);
                        }
                    }
                    else if (focusedItem is Control)
                    {
                        TryMoveFocus(!shiftKeyDown ? FocusNavigationDirection.Down : FocusNavigationDirection.Up);
                    }

                    e.Handled = true;
                    break;

                case VirtualKey.Space:
                case VirtualKey.Enter:
                    // Fire our event using the item with current keyboard focus
                    InvokeItem(focusedItem);
                    e.Handled = true;
                    break;

                default:
                    base.OnKeyDown(e);
                    break;
            }
        }

        /// <summary>
        /// This method is a work-around until the bug in FocusManager.TryMoveFocus is fixed.
        /// </summary>
        /// <param name="direction">Navigated direction.</param>
        private void TryMoveFocus(FocusNavigationDirection direction)
        {
            if (direction == FocusNavigationDirection.Next || direction == FocusNavigationDirection.Previous)
            {
                FocusManager.TryMoveFocus(direction);
            }
            else
            {
                var control = FocusManager.FindNextFocusableElement(direction) as Control;
                control?.Focus(FocusState.Programmatic);
            }
        }

        /// <summary>
        /// Sets a menu item as the active one.
        /// </summary>
        /// <param name="item">the item.</param>
        /// <returns>true when the item was set as active.</returns>
        public bool SetActiveItem(object item)
        {
            if (_splitViewHost == null)
            {
                return false;
            }

            var itemContainer = ContainerFromItem(item);
            InvokeItem(itemContainer);
            return true;
        }

        private void ItemClickedHandler(object sender, ItemClickEventArgs e)
        {
            // Triggered when the item is selected using something other than a keyboard
            var item = ContainerFromItem(e.ClickedItem);
            InvokeItem(item);
        }

        private void InvokeItem(object focusedItem)
        {
            SetSelectedItem(focusedItem as ListViewItem);
            ItemInvoked?.Invoke(this, focusedItem as ListViewItem);

            if (_splitViewHost.IsPaneOpen && (
                _splitViewHost.DisplayMode == SplitViewDisplayMode.CompactOverlay ||
                _splitViewHost.DisplayMode == SplitViewDisplayMode.Overlay))
            {
                _splitViewHost.IsPaneOpen = false;
                var item = focusedItem as ListViewItem;
                item?.Focus(FocusState.Programmatic);
            }
        }

        /// <summary>
        /// Re-size the ListView's Panel when the SplitView is compact so the items
        /// will fit within the visible space and correctly display a keyboard focus rect.
        /// </summary>
        private void OnPaneToggled()
        {
            if (ItemsPanelRoot == null)
            {
                return;
            }

            if (_splitViewHost.IsPaneOpen)
            {
                ItemsPanelRoot.ClearValue(WidthProperty);
                ItemsPanelRoot.ClearValue(HorizontalAlignmentProperty);
            }
            else if (_splitViewHost.DisplayMode == SplitViewDisplayMode.CompactInline ||
                _splitViewHost.DisplayMode == SplitViewDisplayMode.CompactOverlay)
            {
                ItemsPanelRoot.SetValue(WidthProperty, _splitViewHost.CompactPaneLength);
                ItemsPanelRoot.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Left);
            }
        }
    }
}
