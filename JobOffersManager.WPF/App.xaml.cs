using System.Windows;

namespace JobOffersManager.WPF;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        ShutdownMode = ShutdownMode.OnExplicitShutdown;

        var loginWindow = new LoginWindow();
        
        if (loginWindow.ShowDialog() == true)
        {
            ShutdownMode = ShutdownMode.OnMainWindowClose;
            
            var mainWindow = new MainWindow(loginWindow.ApiService);
            MainWindow = mainWindow;
            mainWindow.Show();
        }
        else
        {
            Shutdown();
        }
    }
}
