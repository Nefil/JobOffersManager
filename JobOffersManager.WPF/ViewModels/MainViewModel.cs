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
    public ICommand AddCommand { get; }
    public ICommand DeleteCommand { get; }

    private JobOfferDto? _selectedJob;
    public JobOfferDto? SelectedJob
    {
        get => _selectedJob;
        set => _selectedJob = value;
    }

    public MainViewModel()
    {
        _apiService = new ApiService();

        LoadCommand = new RelayCommand(async _ => await LoadJobs());
        AddCommand = new RelayCommand(async _ => await AddJob());
        DeleteCommand = new RelayCommand(async _ => await DeleteJob());
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

    private async Task AddJob()
    {
        var newJob = new CreateJobOfferDto
        {
            Title = "New Job",
            Location = "Test City",
            Seniority = "Junior",
            Description = "Test description",
            Requirements = "Test requirements",
            Company = "Test Company"
        };

        var created = await _apiService.CreateJobAsync(newJob);

        if (created != null)
        {
            Jobs.Add(created);
        }
    }

    private async Task DeleteJob()
    {
        if (SelectedJob == null)
            return;

        var success = await _apiService.DeleteJobAsync(SelectedJob.Id);

        if (success)
        {
            Jobs.Remove(SelectedJob);
        }
    }
}
