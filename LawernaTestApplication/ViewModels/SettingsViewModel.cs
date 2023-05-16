using System.Collections.Generic;
using System.Linq;
using HSMonitor.Services;
using LawernaTestApplication.Models;
using LawernaTestApplication.ViewModels.Framework;

namespace LawernaTestApplication.ViewModels;

public class SettingsViewModel : DialogScreen
{
    private readonly SettingsService _settingsService;

    public ApplicationSettings ApplicationSettings => _settingsService.Settings;

    public SettingsViewModel(SettingsService settingsService)
    {
        _settingsService = settingsService;
        
        _settingsService.SettingsReset += (_, _) => Refresh();
        _settingsService.SettingsSaved += (_, _) => Refresh();
        _settingsService.SettingsLoaded += (_, _) => Refresh();
    }

    public void Reset() => _settingsService.Reset();

    public void Save()
    {
        _settingsService.Save();
        Close(true);
    }

    public void Cancel()
    {
        _settingsService.Load();
        Close(false);
    }
}