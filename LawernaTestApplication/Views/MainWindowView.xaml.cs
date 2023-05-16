using System.Windows;
using System.Windows.Input;

namespace LawernaTestApplication.Views;

public partial class MainWindowView : Window
{
    public MainWindowView()
    {
        InitializeComponent();
    }
    
    private void HeaderBorder_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
            DragMove();
    }

    private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        if (App.IsHiddenOnLaunch)
            Hide();
    }
        
    private void Show_OnClick(object sender, RoutedEventArgs e) => Show();
    private void Hide_OnClick(object sender, RoutedEventArgs e) => Hide();
    private void Close_OnClick(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
}