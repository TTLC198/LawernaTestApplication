using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Threading;
using AnySoftDesktop.Utils;
using HSMonitor.Services;
using LawernaTestApplication.Models;

namespace LawernaTestApplication.Services;

public class ParserService
{
    private readonly SettingsService _settingsService;
    public WeatherData WeatherData { get; set; }

    public ParserService(SettingsService settingsService)
    {
        _settingsService = settingsService;
        _settingsService.SettingsSaved += SettingsServiceOnSettingsSaved;
    }
    
    private void SettingsServiceOnSettingsSaved(object? sender, EventArgs e)
    {
        if (sender is not SettingsService service) return;
        _settingsService.Settings = service.Settings;
    }

    public event EventHandler? WeatherInformationUpdated;
    
    public async void WeatherInformationUpdate()
    {
        try
        {
            var getProductsRequest = await WebApiService.GetCall($"?q={_settingsService.Settings.City}&units=metric&appid={_settingsService.Settings.ApiKey}");
            if (getProductsRequest.IsSuccessStatusCode)
            {
                var timeoutAfter = TimeSpan.FromMilliseconds(3000);
                using var cancellationTokenSource = new CancellationTokenSource(timeoutAfter);
                var responseStream =
                    await getProductsRequest.Content.ReadAsStreamAsync(cancellationTokenSource.Token);
                var weatherData = await JsonSerializer.DeserializeAsync<WeatherData>(
                    responseStream,
                    CustomJsonSerializerOptions.Options, cancellationToken: cancellationTokenSource.Token);
                WeatherData = weatherData ?? throw new InvalidOperationException("Weather data is null");
            }
            else
            {
                var msg = await getProductsRequest.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"{getProductsRequest.ReasonPhrase}\n{msg}");
            }
        }
        catch
        {
            // ignored
        }
        finally
        {
            OnWeatherInformationUpdated();
        }
    }
    
    private void OnWeatherInformationUpdated()
    {
        WeatherInformationUpdated?.Invoke(this, EventArgs.Empty);
    }
}