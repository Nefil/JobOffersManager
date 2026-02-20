using JobOffersManager.WPF.ViewModels;
using System.Windows;

namespace JobOffersManager.WPF;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}
