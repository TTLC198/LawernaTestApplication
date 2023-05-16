using System.Windows;
using HSMonitor.Services;
using LawernaTestApplication.Services;
using LawernaTestApplication.ViewModels;
using LawernaTestApplication.ViewModels.Framework;
using Stylet;
using StyletIoC;

namespace LawernaTestApplication.Utils;

public class Bootstrapper : Bootstrapper<MainWindowViewModel>
{
    private T GetInstance<T>() => (T) base.GetInstance(typeof(T));

    protected override void ConfigureIoC(IStyletIoCBuilder builder)
    {
        base.ConfigureIoC(builder);
        
        builder.Bind<SettingsService>().ToSelf().InSingletonScope();
        builder.Bind<ParserService>().ToSelf().InSingletonScope();
        builder.Bind<DialogManager>().ToSelf().InSingletonScope();
        
        builder.Bind<IViewModelFactory>().ToAbstractFactory();

        builder.Bind<MainWindowViewModel>().ToSelf().InSingletonScope();
        builder.Bind<SettingsViewModel>().ToSelf().InSingletonScope();
    }

    protected override void Launch()
    {
        // Load settings (this has to come before any view is loaded because bindings are not updated)
        GetInstance<SettingsService>().Load();

        GetInstance<ParserService>().WeatherInformationUpdate();

        // Stylet/WPF is slow, so we preload all dialogs, including descendants, for smoother UX
        _ = GetInstance<DialogManager>().GetViewForDialogScreen(GetInstance<SettingsViewModel>());

        base.Launch();
    }
}