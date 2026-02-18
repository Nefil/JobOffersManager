using System.Collections.ObjectModel;
using System.Windows.Input;
using JobOffersManager.Shared;
using JobOffersManager.WPF.Services;

namespace JobOffersManager.WPF.ViewModels;

public class MainViewModel
{
    private readonly ApiService _apiService;

    public ObservableCollection<JobOfferDto> Jobs { get; set; } = new();

    public ICommand LoadCommand { get; }

    public MainViewModel()
    {
        _apiService = new ApiService();
        LoadCommand = new RelayCommand(async _ => await LoadJobs());
    }

    private async Task LoadJobs()
    {
        var result = await _apiService.GetJobsAsync();
        if (result != null)
        {
            Jobs.Clear();
            foreach (var job in result.Items)
                Jobs.Add(job);
        }
    }
}
