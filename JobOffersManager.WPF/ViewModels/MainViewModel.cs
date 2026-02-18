using JobOffersManager.Shared;
using JobOffersManager.WPF.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace JobOffersManager.WPF.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly ApiService _apiService;

    public ObservableCollection<JobOfferDto> Jobs { get; set; } = new();

    public ICommand LoadCommand { get; }
    public ICommand AddCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand EditCommand { get; }


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
    }

    private async Task LoadJobs()
    {
        try
        {
            var result = await _apiService.GetJobsAsync();
            if (result != null)
            {
                Jobs.Clear();
                foreach (var job in result.Items)
                    Jobs.Add(job);
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
            if (window.ShowDialog() != true)
                return;

            var created = await _apiService.CreateJobAsync(window.CreateDto);

            if (created != null)
                Jobs.Add(created);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}");
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
                // Aktualizacja w kolekcji
                var index = Jobs.IndexOf(SelectedJob);
                Jobs[index] = updated;
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
