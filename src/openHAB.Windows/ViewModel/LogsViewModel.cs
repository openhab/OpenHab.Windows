using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.WinUI;
using Microsoft.Extensions.Logging;
using openHAB.Core;
using openHAB.Core.Common;

namespace openHAB.Windows.ViewModel;

/// <summary>
/// ViewModel for application logs.
/// </summary>
public class LogsViewModel : ViewModelBase<object>, IDisposable
{
    private readonly string _logFilename = $"{DateTime.Now:yyyy-MM-dd}.json";
    private readonly ILogger<LogsViewModel> _logger;
    private FileInfo _logFile;
    private string _logFileContent;
    private FileSystemWatcher _logFileWatcher;
    private ICommand _openLogFileCommand;

    /// <summary>
    /// Initializes a new instance of the <see cref="LogsViewModel" /> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    public LogsViewModel(ILogger<LogsViewModel> logger)
        : base(null)
    {
        _logger = logger;
        _ = LoadLogfileAsync();
    }

    /// <summary>
    /// Gets or sets the content of the log.
    /// </summary>
    /// <value>The content of the log.</value>
    public string LogContent
    {
        get => _logFileContent;
        set => Set(ref _logFileContent, value);
    }

    /// <summary>
    /// Gets the log file name including extension.
    /// </summary>
    /// <value>The log file.</value>
    public string LogFile => _logFile?.Name ?? string.Empty;

    /// <summary>
    /// Asynchronously loads the log file content and registers a file observer.
    /// </summary>
    private async Task LoadLogfileAsync()
    {
        await LoadLogFileContent().ConfigureAwait(false);
        RegisterFileObserver();
    }

    /// <summary>
    /// Asynchronously loads the content of the log file.
    /// </summary>
    private async Task LoadLogFileContent()
    {
        _logger.LogInformation("Load current log file: {LogFilename}", _logFilename);

        string logFolderPath = AppPaths.LogsDirectory;
        if (!Directory.Exists(logFolderPath))
        {
            return;
        }

        string logFilePath = Path.Combine(logFolderPath, _logFilename);
        if (!File.Exists(logFilePath))
        {
            return;
        }

        _logFile = new FileInfo(logFilePath);

        await Task.Run(() =>
        {
            using (FileStream fileStream = File.Open(logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader reader = new StreamReader(fileStream))
            {
                LogContent = reader.ReadToEnd();
            }
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Handles the log file changed event and updates the log content.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private async void LogFile_Changed(object sender, FileSystemEventArgs e)
    {
        await App.DispatcherQueue.EnqueueAsync(async () =>
        {
            using (FileStream fileStream = File.Open(_logFile.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader reader = new StreamReader(fileStream))
            {
                LogContent = reader.ReadToEnd();
            }
        });
    }

    /// <summary>
    /// Registers a file system watcher to observe changes in the log file.
    /// </summary>
    private void RegisterFileObserver()
    {
        _logger.LogInformation("Register FileSystemWatch to load changed log file.");

        _logFileWatcher = new FileSystemWatcher
        {
            Path = Path.GetDirectoryName(_logFile.FullName),
            Filter = _logFilename
        };

        _logFileWatcher.Changed += LogFile_Changed;
        _logFileWatcher.EnableRaisingEvents = true;
    }

    #region Command

    /// <summary>
    /// Gets the command to open log files directory.
    /// </summary>
    /// <value>The open log file command.</value>
    public ICommand OpenLogFileCommand => _openLogFileCommand ??= new ActionCommand(ExecuteOpenLogFileCommand, CanExecuteOpenLogFileCommand);

    /// <summary>
    /// Determines whether the open log file command can execute.
    /// </summary>
    /// <param name="arg">The command argument.</param>
    /// <returns><c>true</c> if the command can execute; otherwise, <c>false</c>.</returns>
    private bool CanExecuteOpenLogFileCommand(object arg) => true;

    /// <summary>
    /// Executes the open log file command.
    /// </summary>
    /// <param name="obj">The command parameter.</param>
    private void ExecuteOpenLogFileCommand(object obj)
    {
        _logger.LogInformation("Open log files directory: '{LogFolderPath}'", AppPaths.LogsDirectory);
        Process.Start(new ProcessStartInfo
        {
            FileName = AppPaths.LogsDirectory,
            UseShellExecute = true,
            Verb = "open"
        });
    }

    #endregion Command

    #region IDisposable Support

    private bool disposedValue = false; // To detect redundant calls

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _logFileWatcher?.Dispose();
            }

            disposedValue = true;
        }
    }
    #endregion IDisposable Support
}
