using Microsoft.Extensions.Logging;
using openHAB.Core.Client;
using openHAB.Core.Model;
using openHAB.Core.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openHAB.Windows.ViewModel
{
    public class MainUIViewModel : ViewModelBase<object>
    {
        private string _mainUiUrl;
        private ILogger<MainUIViewModel> _logger;

        public MainUIViewModel( ILogger<MainUIViewModel> logger)
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
}
