using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using LawernaTestApplication;
using LawernaTestApplication.Models;
using LawernaTestApplication.Utils;
using LawernaTestApplication.ViewModels;
using LawernaTestApplication.ViewModels.Framework;

namespace HSMonitor.Services;

public class SettingsService
{
    private readonly IViewModelFactory _viewModelFactory;
    private readonly DialogManager _dialogManager;
    
    public event EventHandler? SettingsReset;
    
    public event EventHandler? SettingsLoaded;
    
    public event EventHandler? SettingsSaved;
    
    public ApplicationSettings Settings { get; set; } = null!;

    private readonly string _configurationPath = Path.Combine(App.ExecutableDirPath, "appsettings.json");

    public SettingsService(IViewModelFactory viewModelFactory, DialogManager dialogManager)
    {
        _viewModelFactory = viewModelFactory;
        _dialogManager = dialogManager;
        Load();
    }
    
    public void Reset()
    {
        Settings = new ApplicationSettings()
        {
            City = null,
            ApiKey = null,
            UpdateInterval = 1000
        };
        Save();
        SettingsReset?.Invoke(this, EventArgs.Empty);
    }

    public void Load()
    {
        try
        {
            var settingsJson = File.ReadAllText(_configurationPath);
            Settings = JsonSerializer.Deserialize<ApplicationSettings>(settingsJson) ?? throw new InvalidOperationException();
            SettingsLoaded?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception exception)
        {
            Task.Run(async () =>
            {
                var messageBoxDialog = _viewModelFactory.CreateMessageBoxViewModel(
                    title: "Some error has occurred",
                    message: $@"
An error has occurred, the error text is shown below
{exception.Message}".Trim(),
                    okButtonText: "OK",
                    cancelButtonText: null
                );

                if (await _dialogManager.ShowDialogAsync(messageBoxDialog) == true)
                    Application.Current.Shutdown();
            });
        }
    }

    public void Save()
    {
        Settings.JsonToFile(_configurationPath);
        
        SettingsSaved?.Invoke(this, EventArgs.Empty);
    }
}