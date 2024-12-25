using Microsoft.UI.Xaml.Data;
using openHAB.Core.Client.Models;
using System;

namespace openHAB.Windows.Converters;

/// <summary>
/// Converts objects to sitemaps, for use in compiled bindings.
/// </summary>
public class ObjectToSitemapConverter : IValueConverter
{
    /// <inheritdoc/>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return value as Sitemap;
    }

    /// <inheritdoc/>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return value as Sitemap;
    }
}
