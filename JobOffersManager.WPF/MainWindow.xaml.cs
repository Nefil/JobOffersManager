using JobOffersManager.WPF.Services;
using JobOffersManager.WPF.ViewModels;
using System.Windows;

namespace JobOffersManager.WPF;

public partial class MainWindow : Window
{
    public MainWindow(ApiService apiService)
    {
        InitializeComponent();
        DataContext = new MainViewModel(apiService);
    }
}
