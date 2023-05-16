using System.Windows;
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
        
        builder.Bind<DialogManager>().ToSelf().InSingletonScope();
        
        builder.Bind<IViewModelFactory>().ToAbstractFactory();

        builder.Bind<MainWindowViewModel>().ToSelf().InSingletonScope();
    }
    
    protected override void Launch()
    {
        
        base.Launch();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
    }
}