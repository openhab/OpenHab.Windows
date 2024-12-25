using Microsoft.Extensions.Logging;
using openHAB.Core.Client;

namespace openHAB.Windows.ViewModel;

public class MainUIViewModel : ViewModelBase<object>
{
    private string _mainUiUrl;
    private readonly ILogger<MainUIViewModel> _logger;

    public MainUIViewModel(ILogger<MainUIViewModel> logger)
        : base(new object())
    {
        _logger = logger;

        MainUIUrl = OpenHABHttpClient.BaseUrl;
    }

    public string MainUIUrl
    {
        get => _mainUiUrl;
        set => Set(ref _mainUiUrl, value);
    }
}
