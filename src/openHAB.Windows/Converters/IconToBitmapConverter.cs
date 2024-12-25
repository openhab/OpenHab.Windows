using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Imaging;
using openHAB.Core.Model;
using openHAB.Windows.ViewModel;

namespace openHAB.Windows.Converters;

/// <summary>
/// Converts an icon path to a bitmap object.
/// </summary>
public class IconToBitmapConverter : IValueConverter
{
    /// <inheritdoc/>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        IOptions<Settings> settingsOptions = Program.Host.Services.GetRequiredService<IOptions<Settings>>();
        Settings settings = settingsOptions.Value;

        WidgetViewModel? widget = value as WidgetViewModel;
        if (widget == null || string.IsNullOrEmpty(widget.IconPath))
        {
            return null;
        }

        if (settings.UseSVGIcons)
        {
            return new SvgImageSource(new Uri(widget.IconPath));
        }
        else
        {
            return new BitmapImage(new Uri(widget.IconPath));
        }
    }

    /// <inheritdoc/>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotSupportedException();
    }
}
