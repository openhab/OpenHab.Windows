﻿using System.ComponentModel;
using Microsoft.UI.Xaml;

namespace openHAB.Core.Model;

/// <summary>
/// Specifies the application theme.
/// </summary>
public enum AppTheme
{
    /// <summary>
    /// Light theme.
    /// </summary>
    Light = ApplicationTheme.Light,

    /// <summary>
    /// Dark theme.
    /// </summary>
    Dark = ApplicationTheme.Dark,

    /// <summary>
    /// System default theme.
    /// </summary>
    System = 2
}

public static class AppThemeExtension
{
    public static ApplicationTheme ConvertToApplicationTheme(this AppTheme theme)
    {
        switch (theme)
        {
            case AppTheme.Light:
                return ApplicationTheme.Light;
            case AppTheme.Dark:
                return ApplicationTheme.Dark;
            case AppTheme.System:
            default:
                throw new InvalidEnumArgumentException(nameof(theme));
        }
    }
}
