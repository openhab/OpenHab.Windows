using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using openHAB.Core.Client.Contracts;
using openHAB.Core.Client.Messages;
using openHAB.Core.Client.Models;
using openHAB.Core.Common;
using openHAB.Core.Messages;
using openHAB.Core.Model;
using openHAB.Core.Services;
using openHAB.Core.Services.Contracts;
using openHAB.Windows.Messages;
using openHAB.Windows.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace openHAB.Windows.ViewModel;

/// <summary>
/// Collects and formats all the data for starting the application.
/// </summary>
public class MainViewModel : ViewModelBase<object>
{
    private readonly IOpenHABClient _openHABClient;
    private readonly IOptions<Settings> _settingsOptions;
    private readonly ISettingsService _settingsService;
    private readonly ILogger<MainViewModel> _logger;
    private readonly SitemapService _sitemapManager;

    private ObservableCollection<WidgetViewModel> _breadcrumbItems;
    private bool _isDataLoading;
    private object _selectedMenuItem;
    private Sitemap _selectedSitemap;
    private ObservableCollection<Sitemap> _sitemaps;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainViewModel"/> class.
    /// </summary>
    /// <param name="openHABClient">The OpenHAB client.</param>
    /// <param name="settingsService">Setting service instance.</param>
    /// <param name="logger">Logger class instance.</param>
    public MainViewModel(IOpenHABClient openHABClient,
        ISettingsService settingsService,
        IOptions<Settings> settingsOptions,
        SitemapService sitemapManager,
        ILogger<MainViewModel> logger)
        : base(new object())
    {
        _logger = logger;

        _openHABClient = openHABClient;
        _settingsOptions = settingsOptions;
        _settingsService = settingsService;
        _breadcrumbItems = new ObservableCollection<WidgetViewModel>();
        _sitemapManager = sitemapManager;

        StrongReferenceMessenger.Default.Register<DataOperation>(this, async (obj, operation)
            => await DataOperationStateAsync(operation));
    }

    /// <summary>
    /// Gets or sets the items for the breadcrumb.
    /// </summary>
    /// <value>The breadcrumb items.</value>
    public ObservableCollection<WidgetViewModel> BreadcrumbItems
    {
        get => _breadcrumbItems;
        set => Set(ref _breadcrumbItems, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether data is loaded from an OpenHAB instance.
    /// </summary>
    /// <value>
    ///   <c>true</c> if data will loaded; otherwise, <c>false</c>.</value>
    public bool IsDataLoading
    {
        get => _isDataLoading;
        set => Set(ref _isDataLoading, value);
    }

    /// <summary>
    /// Gets or sets the selected menu item.
    /// </summary>
    public object SelectedMenuItem
    {
        get
        {
            return _selectedMenuItem;
        }

        set
        {
            Sitemap sitemapInfo = value as Sitemap;
            if (sitemapInfo != null && SelectedSitemap != value)
            {
                SelectedSitemap = sitemapInfo;
            }

            Set(ref _selectedMenuItem, value);
        }
    }

    /// <summary>
    /// Gets or sets the sitemap currently selected by the user.
    /// </summary>
    public Sitemap SelectedSitemap
    {
        get
        {
            return _selectedSitemap;
        }

        set
        {
            if (_selectedSitemap != value && value != null)
            {
                StrongReferenceMessenger.Default.Unregister<WidgetNavigationMessage, string>(this, value.Name);
            }

            if (Set(ref _selectedSitemap, value))
            {
                if (_selectedSitemap != null)
                {
                    Settings settings = _settingsOptions.Value;
                    settings.LastSitemap = _selectedSitemap.Name;
                    _settingsService.Save(settings);

                    StrongReferenceMessenger.Default.Register<WidgetNavigationMessage, string>(this, SelectedSitemap.Name, (obj, operation)
                         => WidgetNavigatedEvent());
                }

                BreadcrumbItems.Clear();
                SelectedMenuItem = value;
            }

            OnPropertyChanged(nameof(SelectedSitemap));
        }
    }

    /// <summary>
    /// Gets or sets a collection of OpenHAB sitemaps.
    /// </summary>
    public ObservableCollection<Sitemap> Sitemaps
    {
        get => _sitemaps;
        set => Set(ref _sitemaps, value);
    }

    #region Sitemaps Refresh Command

    private ActionCommand _refreshCommand;

    /// <summary>Gets the command to refresh sitemap and widget data.</summary>
    /// <value>The refresh command.</value>
    public ActionCommand RefreshCommand => _refreshCommand ?? (_refreshCommand = new ActionCommand(ExecuteRefreshCommandAsync, CanExecuteRefreshCommand));

    private bool CanExecuteRefreshCommand(object arg)
    {
        return !IsDataLoading;
    }

    private async void ExecuteRefreshCommandAsync(object obj)
    {
        await LoadSitemapsAndItemData().ConfigureAwait(false);
    }

    #endregion

    #region Load sitemap and Data

    /// <summary>
    /// Loads the sitemap data.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task LoadSitemapsAndItemData()
    {
        using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource())
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                await LoadData(cancellationTokenSource.Token).ConfigureAwait(false);
            });
        }
    }

    private async void CancelSyncCallbackAsync()
    {
        await App.DispatcherQueue.EnqueueAsync(() =>
        {
            Sitemaps?.Clear();
            SelectedSitemap = null;
            StrongReferenceMessenger.Default.Send<DataOperation>(new DataOperation(OperationState.Completed));
        });
    }

    private async Task LoadData(CancellationToken loadCancellationToken)
    {
        loadCancellationToken.Register(CancelSyncCallbackAsync);

        try
        {
            if (IsDataLoading)
            {
                return;
            }

            _logger.LogInformation("Load available sitemaps and their items");

            StrongReferenceMessenger.Default.Send<DataOperation>(new DataOperation(OperationState.Started));
            Sitemaps?.Clear();

            if (loadCancellationToken.IsCancellationRequested)
            {
                return;
            }

            List<Sitemap> sitemaps = await _sitemapManager.GetSitemapsAsync(loadCancellationToken);
            if (sitemaps == null)
            {
                StrongReferenceMessenger.Default.Send(new FireInfoMessage(MessageType.NotConfigured));
                return;
            }

            Sitemaps = new ObservableCollection<Sitemap>(sitemaps);
            _openHABClient.StartItemUpdates(loadCancellationToken);

            SelectedSitemap = OpenLastOrDefaultSitemap();
        }
        catch (OpenHABException ex)
        {
            _logger.LogError(ex, "Load data failed.");
            StrongReferenceMessenger.Default.Send(new ConnectionErrorMessage(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Load data failed.");
        }
        finally
        {
            StrongReferenceMessenger.Default.Send<DataOperation>(new DataOperation(OperationState.Completed));
            RefreshCommand.InvokeCanExecuteChanged(null);
        }
    }

    private Sitemap OpenLastOrDefaultSitemap()
    {
        Settings settings = _settingsOptions.Value;
        string sitemapName = settings.LastSitemap;

        if (string.IsNullOrWhiteSpace(sitemapName))
        {
            _logger.LogInformation("No sitemap was selected in the past -> Pick first entry from list");

            return Sitemaps.FirstOrDefault();
        }

        Sitemap selectedSitemap = Sitemaps.FirstOrDefault(x => x.Name == sitemapName);
        if (selectedSitemap == null)
        {
            _logger.LogInformation($"Unable to find sitemap '{sitemapName}' -> Pick first entry from list");
            return Sitemaps.FirstOrDefault();
        }

        return selectedSitemap;
    }

    #endregion

    #region Events

    private async Task DataOperationStateAsync(DataOperation operation)
    {
        await App.DispatcherQueue.EnqueueAsync(() =>
        {
            switch (operation.State)
            {
                case OperationState.Started:
                    IsDataLoading = true;
                    break;

                case OperationState.Completed:
                    IsDataLoading = false;
                    break;
            }
        });
    }

    private void WidgetNavigatedEvent()
    {
        BreadcrumbItems?.Clear();
        BreadcrumbItems?.AddRange(WidgetNavigationService.Widgets);
        OnPropertyChanged(nameof(BreadcrumbItems));
    }

    #endregion
}
