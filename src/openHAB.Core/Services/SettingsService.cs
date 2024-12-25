using System.IO;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using openHAB.Core.Model;
using openHAB.Core.Services.Contracts;

namespace openHAB.Core.Services;

/// <summary>
/// Service that handles all settings operations.
/// </summary>
public class SettingsService : ISettingsService
{
    private readonly ILogger<SettingsService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsService"/> class.
    /// </summary>
    public SettingsService(ILogger<SettingsService> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public bool Save(Settings settings)
    {
        try
        {
            _logger.LogInformation("Save settings to disk");

            string settingsContent = JsonSerializer.Serialize(settings);
            File.WriteAllText(AppPaths.SettingsFilePath, settingsContent, Encoding.UTF8);

            return true;
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Failed to save settings.");
            return false;
        }
    }
}
