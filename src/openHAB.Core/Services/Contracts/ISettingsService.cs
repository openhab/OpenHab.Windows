using openHAB.Core.Model;

namespace openHAB.Core.Services.Contracts;

/// <summary>
/// Service to fetch / save user-defined settings.
/// </summary>
public interface ISettingsService
{
    /// <summary>
    /// Persists the current settings.
    /// </summary>
    /// <param name="settings">Current settings.</param>
    /// <returns>true when settings stored successful to disk.</returns>
    bool Save(Settings settings);
}
