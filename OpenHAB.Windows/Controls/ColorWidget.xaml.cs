﻿using System;
using System.Globalization;
using GalaSoft.MvvmLight.Messaging;
using OpenHAB.Core.Common;
using OpenHAB.Core.Messages;
using Windows.UI;
using Windows.UI.Xaml;

namespace OpenHAB.Windows.Controls
{
    /// <summary>
    /// Widget control that represents an OpenHAB text.
    /// </summary>
    public sealed partial class ColorWidget : WidgetBase
    {
        private Color _selectedColor;
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorWidget"/> class.
        /// </summary>
        public ColorWidget()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        /// <summary>
        /// Gets or sets the color currently selected in the color picker widget.
        /// </summary>
        public Color SelectedColor
        {
            get => _selectedColor;

            set
            {
                if (_selectedColor == value)
                {
                    return;
                }

                _selectedColor = value;
                
                RaisePropertyChanged();
            }
        }


        internal override void SetState()
        {
            ClrPicker.ColorChanged -= ClrPicker_ColorChanged;
            var rgbString = Widget.Item?.State.Split(',');

            if (rgbString == null || rgbString.Length == 0)
            {
                return;
            }
            double h = Convert.ToDouble(rgbString[0], CultureInfo.InvariantCulture);
            double s = Convert.ToDouble(rgbString[1], CultureInfo.InvariantCulture) /100;
            double v = Convert.ToDouble(rgbString[2], CultureInfo.InvariantCulture) / 100;

            SelectedColor = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.FromHsv(h,s,v);
            ClrPicker.ColorChanged += ClrPicker_ColorChanged;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            SetState();
            
        }

        private void ClrPicker_ColorChanged(global::Windows.UI.Xaml.Controls.ColorPicker sender, global::Windows.UI.Xaml.Controls.ColorChangedEventArgs args)
        {
            var hsvclr = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToHsv(sender.Color);
            Messenger.Default.Send(new TriggerCommandMessage(Widget.Item, $"{hsvclr.H.ToString(CultureInfo.InvariantCulture)},{(hsvclr.S*100).ToString(CultureInfo.InvariantCulture)}, {((hsvclr.V)*100).ToString(CultureInfo.InvariantCulture)}"));
        }
    }
}
