using JobOffersManager.WPF.Services;
using System.Windows;

namespace JobOffersManager.WPF;

public partial class LoginWindow : Window
{
    private readonly ApiService _apiService;

    public ApiService ApiService => _apiService;
    public bool Success { get; private set; }

    public LoginWindow()
    {
        InitializeComponent();
        _apiService = new ApiService();
    }

    private async void Login_Click(object sender, RoutedEventArgs e)
    {
        var username = UsernameTextBox.Text;
        var password = PasswordBox.Password;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            MessageBox.Show("Please enter username and password", "Validation Error",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            var success = await _apiService.LoginAsync(username, password);

            if (success)
            {
                Success = true;
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Invalid credentials", "Login Failed",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Login error: {ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        Success = false;
        DialogResult = false;
        Close();
    }
}