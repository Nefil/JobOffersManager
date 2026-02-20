using JobOffersManager.Shared;
using JobOffersManager.WPF.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using JobOffersManager.WPF.Helpers;

namespace JobOffersManager.WPF.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly ApiService _apiService;

    public ObservableCollection<JobOfferDto> Jobs { get; set; } = new();

    public ICommand LoadCommand { get; }
    public ICommand AddCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand NextPageCommand { get; }
    public ICommand PreviousPageCommand { get; }
    public ICommand SearchCommand { get; }

    private JobOfferDto? _selectedJob;
    public JobOfferDto? SelectedJob
    {
        get => _selectedJob;
        set
        {
            if (_selectedJob != value)
            {
                _selectedJob = value;
                OnPropertyChanged();
            }
        }
    }

    private string? _filterLocation;
    public string? FilterLocation
    {
        get => _filterLocation;
        set
        {
            _filterLocation = value;
            OnPropertyChanged();
        }
    }

    private string? _filterSeniority;
    public string? FilterSeniority
    {
        get => _filterSeniority;
        set
        {
            _filterSeniority = value;
            OnPropertyChanged();
        }
    }

    private int _currentPage = 1;
    public int CurrentPage
    {
        get => _currentPage;
        set
        {
            _currentPage = value;
            OnPropertyChanged();
        }
    }

    private int _totalPages;
    public int TotalPages
    {
        get => _totalPages;
        set
        {
            _totalPages = value;
            OnPropertyChanged();
        }
    }

    private const int PageSize = 5;

    public MainViewModel()
    {
        _apiService = new ApiService();

        LoadCommand = new RelayCommand(async _ => await LoadJobs());
        AddCommand = new RelayCommand(async _ => await AddJob());
        DeleteCommand = new RelayCommand(
            async _ => await DeleteJob(SelectedJob),
            _ => SelectedJob != null);

        EditCommand = new RelayCommand(
            async _ => await EditJob(),
            _ => SelectedJob != null);
        NextPageCommand = new RelayCommand(async _ => await NextPage());
        PreviousPageCommand = new RelayCommand(async _ => await PreviousPage());
        SearchCommand = new RelayCommand(async _ =>
        {
            CurrentPage = 1;
            await LoadJobs();
        });
    }

    private async Task LoadJobs()
    {
        try
        {
            var result = await _apiService.GetJobsAsync(
                CurrentPage,
                PageSize,
                FilterLocation,
                FilterSeniority);

            if (result != null)
            {
                Jobs.Clear();

                foreach (var job in result.Items)
                    Jobs.Add(job);

                TotalPages = result.TotalPages;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading jobs: {ex.Message}");
        }
    }

    private async Task AddJob()
    {
        try
        {
            var window = new AddEditJobWindow();

            if (window.ShowDialog() == true)
            {
                var created = await _apiService.CreateJobAsync(window.CreateDto);

                if (created != null)
                {
                    await LoadJobs(); // Refresh list
                    MessageBox.Show("Job added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Failed to add job. Check if API is running on https://localhost:7101\n\nDetails in Output window (View > Output)", 
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error adding job: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task DeleteJob(object? parameter)
    {
        if (parameter is not JobOfferDto job)
            return;

        var confirm = MessageBox.Show(
            $"Are you sure you want to delete '{job.Title}'?",
            "Confirm delete",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (confirm != MessageBoxResult.Yes)
            return;

        var success = await _apiService.DeleteJobAsync(job.Id);

        if (success)
            Jobs.Remove(job);
    }

    private async Task EditJob()
    {
        if (SelectedJob == null)
            return;

        var window = new AddEditJobWindow(SelectedJob);

        if (window.ShowDialog() == true)
        {
            var updated = await _apiService.UpdateJobAsync(
                SelectedJob.Id,
                window.UpdateDto);

            if (updated != null)
            {
                // Update in collection
                var index = Jobs.IndexOf(SelectedJob);
                Jobs[index] = updated;
            }
        }
    }

    private async Task NextPage()
    {
        if (CurrentPage < TotalPages)
        {
            CurrentPage++;
            await LoadJobs();
        }
    }

    private async Task PreviousPage()
    {
        if (CurrentPage > 1)
        {
            CurrentPage--;
            await LoadJobs();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
