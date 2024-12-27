using Microsoft.Extensions.Logging;
using openHAB.Core.Client.Connection.Contracts;

namespace openHAB.Windows.ViewModel;

public class MainUIViewModel : ViewModelBase<object>
{
    private string _mainUiUrl;
    private readonly ILogger<MainUIViewModel> _logger;

    public MainUIViewModel(IConnectionService connectionService, ILogger<MainUIViewModel> logger)
        : base(new object())
    {
        _logger = logger;

        MainUIUrl = connectionService.CurrentConnection.MainUIUrl;
    }

    public string MainUIUrl
    {
        get => _mainUiUrl;
        set => Set(ref _mainUiUrl, value);
    }
}
