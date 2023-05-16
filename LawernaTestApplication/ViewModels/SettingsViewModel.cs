using System.Collections.Generic;
using System.Linq;
using HSMonitor.Services;
using LawernaTestApplication.ViewModels.Framework;

namespace LawernaTestApplication.ViewModels;

public class SettingsViewModel : DialogScreen
{
    private readonly SettingsService _settingsService;

    public SettingsViewModel(SettingsService settingsService)
    {
        _settingsService = settingsService;
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