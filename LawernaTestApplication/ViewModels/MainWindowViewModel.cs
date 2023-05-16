using System;
using System.ComponentModel;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using HSMonitor.Services;
using LawernaTestApplication.Models;
using LawernaTestApplication.Services;
using LawernaTestApplication.Utils;
using LawernaTestApplication.ViewModels.Framework;
using Stylet;

namespace LawernaTestApplication.ViewModels;

public class MainWindowViewModel : Screen, INotifyPropertyChanged
{
    private readonly IViewModelFactory _viewModelFactory;
    private readonly DialogManager _dialogManager;
    private readonly SettingsService _settingsService;
    private readonly ParserService _parserService;

    private DispatcherTimer _updateWeatherInformationTimer = null!;

    private WeatherData _weatherData;

    public WeatherData WeatherData
    {
        get => _weatherData;
        set
        {
            _weatherData = value;
            OnPropertyChanged(nameof(WeatherData));
        }
    }

    public MainWindowViewModel(IViewModelFactory viewModelFactory, DialogManager dialogManager, SettingsService settingsService, ParserService parserService)
    {
        _viewModelFactory = viewModelFactory;
        _dialogManager = dialogManager;
        _settingsService = settingsService;
        _parserService = parserService;
        
        DisplayName = $"{App.Name} v{App.VersionString}";
        _parserService.WeatherInformationUpdated += OnWeatherInformationUpdated;
    }

    public async void OnViewFullyLoaded()
    {
        _updateWeatherInformationTimer = new DispatcherTimer(
            priority: DispatcherPriority.Background,
            interval: TimeSpan.FromMilliseconds(
                _settingsService.Settings.UpdateInterval == 0
                    ? 500
                    : _settingsService.Settings.UpdateInterval),
            callback: (_, _) => { _parserService.WeatherInformationUpdate(); },
            dispatcher: Dispatcher.FromThread(Thread.CurrentThread) ?? throw new InvalidOperationException()
        );

        _settingsService.SettingsSaved += (_, _) =>
        {
            _updateWeatherInformationTimer.Interval = TimeSpan.FromMilliseconds(
                _settingsService.Settings.UpdateInterval == 0
                    ? 500 
                    : _settingsService.Settings.UpdateInterval);
        };
        
        if (_settingsService.Settings is {ApiKey: null} and {City: null})
            await FirstStart();
    }

    private void OnWeatherInformationUpdated(object? sender, EventArgs e)
    {
        if (sender is not ParserService parser)
            return;
        WeatherData = parser.WeatherData;
    }
    
    private async Task FirstStart()
    {
        var messageBoxDialog = _viewModelFactory.CreateMessageBoxViewModel(
            title: "First usage",
            message: $@"
Please enter the details of the city, the data of your api key to get weather information.

Click on OK to go to settings.".Trim(),
            okButtonText: "OK",
            cancelButtonText: "CANCEL"
        );

        if (await _dialogManager.ShowDialogAsync(messageBoxDialog) == true)
        {
            ShowSettings();
        }
    }

    public async void ShowSettings()
    {
        _settingsService.Load();
        await _dialogManager.ShowDialogAsync(_viewModelFactory.CreateSettingsViewModel());
    }

    public void ShowAbout() => OpenUrl.Open(App.GitHubProjectUrl);

    private void Exit() => Application.Current.Shutdown();
}