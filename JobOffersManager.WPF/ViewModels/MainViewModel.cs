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

    public ObservableCollection<JobOfferDto> Jobs { get; } = new();

    public ICommand LoadCommand { get; }
    public ICommand AddCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand NextPageCommand { get; }
    public ICommand PreviousPageCommand { get; }
    public ICommand SearchCommand { get; }

    public bool IsAdmin => _apiService.Role == "Admin";

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
                RaiseCanExecuteChanged();
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

    public MainViewModel(ApiService apiService)
    {
        _apiService = apiService;

        LoadCommand = new RelayCommand(async _ => await LoadJobs());
        AddCommand = new RelayCommand(async _ => await AddJob(), _ => IsAdmin);
        DeleteCommand = new RelayCommand(async _ => await DeleteJob(), _ => IsAdmin && SelectedJob != null);
        EditCommand = new RelayCommand(async _ => await EditJob(), _ => IsAdmin && SelectedJob != null);
        NextPageCommand = new RelayCommand(async _ => await NextPage());
        PreviousPageCommand = new RelayCommand(async _ => await PreviousPage());
        SearchCommand = new RelayCommand(async _ => await LoadJobs());
    }

    private void RaiseCanExecuteChanged()
    {
        (DeleteCommand as RelayCommand)?.RaiseCanExecuteChanged();
        (EditCommand as RelayCommand)?.RaiseCanExecuteChanged();
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

            if (result == null)
                return;

            Jobs.Clear();

            foreach (var job in result.Items)
                Jobs.Add(job);

            TotalPages = result.TotalPages;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading jobs: {ex.Message}");
        }
    }

    private async Task AddJob()
    {
        if (!IsAdmin)
        {
            MessageBox.Show("You don't have permission to add jobs.");
            return;
        }

        var window = new AddEditJobWindow();

        if (window.ShowDialog() != true)
            return;

        try
        {
            var created = await _apiService.CreateJobAsync(window.CreateDto);

            if (created == null)
            {
                MessageBox.Show("Unauthorized. Token may be missing.");
                return;
            }

            await LoadJobs();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error adding job: {ex.Message}");
        }
    }

    private async Task DeleteJob()
    {
        if (!IsAdmin)
        {
            MessageBox.Show("You don't have permission to delete jobs.");
            return;
        }

        if (SelectedJob == null)
        {
            MessageBox.Show("Please select a job offer to delete.");
            return;
        }

        var confirm = MessageBox.Show(
            $"Delete '{SelectedJob.Title}'?",
            "Confirm",
            MessageBoxButton.YesNo);

        if (confirm != MessageBoxResult.Yes)
            return;

        try
        {
            var success = await _apiService.DeleteJobAsync(SelectedJob.Id);

            if (!success)
            {
                MessageBox.Show("Failed to delete job. Please check your permissions.");
                return;
            }

            Jobs.Remove(SelectedJob);
            SelectedJob = null;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting job: {ex.Message}");
        }
    }

    private async Task EditJob()
    {
        if (!IsAdmin || SelectedJob == null)
            return;

        var window = new AddEditJobWindow(SelectedJob);

        if (window.ShowDialog() != true)
            return;

        try
        {
            var updated = await _apiService.UpdateJobAsync(
                SelectedJob.Id,
                window.UpdateDto);

            if (updated == null)
            {
                MessageBox.Show("Unauthorized.");
                return;
            }

            var index = Jobs.IndexOf(SelectedJob);
            Jobs[index] = updated;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating job: {ex.Message}");
        }
    }

    private async Task NextPage()
    {
        if (CurrentPage >= TotalPages)
            return;

        CurrentPage++;
        await LoadJobs();
    }

    private async Task PreviousPage()
    {
        if (CurrentPage <= 1)
            return;

        CurrentPage--;
        await LoadJobs();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
