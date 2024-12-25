using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using openHAB.Core.Client.Models;
using openHAB.Core.Model;
using openHAB.Core.Services.Contracts;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.System.UserProfile;

namespace openHAB.Core.Services;

/// <inheritdoc/>
public class AppManager : IAppManager
{
    private readonly ILogger<AppManager> _logger;
    private readonly IOptions<Settings> _options;
    private readonly string _startupId = "openHABStartupId";

    /// <summary>
    /// Initializes a new instance of the <see cref="AppManager" /> class.
    /// </summary>
    public AppManager(IOptions<Settings> options, ILogger<AppManager> logger)
    {
        _logger = logger;
        _options = options;
    }

    /// <inheritdoc />
    public OpenHABVersion ServerVersion
    {
        get; set;
    }

    /// <inheritdoc/>
    public async Task<bool> CanEnableAutostart()
    {
        StartupTask startupTask = await StartupTask.GetAsync(_startupId);
        return !(startupTask.State == StartupTaskState.DisabledByPolicy || startupTask.State == StartupTaskState.DisabledByUser);
    }

    /// <inheritdoc/>
    public async Task<bool> IsStartupEnabled()
    {
        StartupTask startupTask = await StartupTask.GetAsync(_startupId);
        return startupTask.State == StartupTaskState.Enabled || startupTask.State == StartupTaskState.EnabledByPolicy;
    }

    /// <inheritdoc />
    public void SetProgramLanguage(string langcode)
    {
        if (string.IsNullOrEmpty(langcode))
        {
            Settings settings = _options.Value;
            langcode = settings.AppLanguage;
        }

        if (!string.IsNullOrEmpty(langcode))
        {
            CultureInfo.CurrentCulture = new CultureInfo(langcode);
        }
        else
        {
            string userLanguage = GlobalizationPreferences.Languages[0];
            CultureInfo.CurrentCulture = new CultureInfo(userLanguage);
        }
    }

    /// <inheritdoc/>
    public async Task ToggleAutostart()
    {
        StartupTask startupTask = await StartupTask.GetAsync(_startupId);
        switch (startupTask.State)
        {
            case StartupTaskState.DisabledByPolicy:
            case StartupTaskState.DisabledByUser:
            case StartupTaskState.Disabled:
                StartupTaskState newState = await startupTask.RequestEnableAsync();
                _logger.LogInformation($"App autostart: {newState.ToString()}");
                break;

            case StartupTaskState.EnabledByPolicy:
            case StartupTaskState.Enabled:
                startupTask.Disable();
                _logger.LogInformation($"App autostart: disabled");
                break;
        }
    }
}
