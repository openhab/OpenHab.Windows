using openHAB.Core.Client.Models;
using System.Threading.Tasks;

namespace openHAB.Core.Services.Contracts;

/// <summary>
/// Manages app behavior like autostart.
/// </summary>
public interface IAppManager
{
    /// <summary>
    /// Gets or sets the version of openHAB that's running on the server.
    /// </summary>
    OpenHABVersion ServerVersion
    {
        get; set;
    }

    /// <summary>
    /// Determines if the autostart can be enabled on system logon.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a boolean value indicating whether autostart can be enabled.
    /// </returns>
    Task<bool> CanEnableAutostart();

    /// <summary>
    /// Determines whether autostartup on system logon is enabled.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a boolean value indicating whether autostart is enabled.
    /// </returns>
    Task<bool> IsStartupEnabled();

    /// <summary>
    /// Sets the program language.
    /// </summary>
    /// <param name="langcode">The language code to set the program language to.</param>
    void SetProgramLanguage(string langcode);

    /// <summary>
    /// Toggles the autostart.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// </returns>
    Task ToggleAutostart();
}
