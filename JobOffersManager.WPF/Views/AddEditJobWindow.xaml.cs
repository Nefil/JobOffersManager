using JobOffersManager.Shared;
using System.Windows;

namespace JobOffersManager.WPF;

public partial class AddEditJobWindow : Window
{
    public CreateJobOfferDto CreateDto { get; private set; }
    public UpdateJobOfferDto UpdateDto { get; private set; }

    public bool IsEditMode { get; }

    public AddEditJobWindow()
    {
        InitializeComponent();
        CreateDto = new CreateJobOfferDto();
        DataContext = CreateDto;
        IsEditMode = false;
        Title = "Add New Job Offer";
    }

    public AddEditJobWindow(JobOfferDto existingJob)
    {
        InitializeComponent();

        UpdateDto = new UpdateJobOfferDto
        {
            Title = existingJob.Title,
            Location = existingJob.Location,
            Seniority = existingJob.Seniority,
            Description = existingJob.Description,
            Requirements = existingJob.Requirements,
            Company = existingJob.Company
        };

        DataContext = UpdateDto;
        IsEditMode = true;
        Title = "Edit Job Offer";
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        if (IsEditMode)
        {
            if (string.IsNullOrWhiteSpace(UpdateDto.Title))
            {
                MessageBox.Show("Title is required", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(UpdateDto.Location))
            {
                MessageBox.Show("Location is required", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(UpdateDto.Seniority))
            {
                MessageBox.Show("Seniority is required", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(UpdateDto.Company))
            {
                MessageBox.Show("Company is required", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(UpdateDto.Description))
            {
                MessageBox.Show("Description is required", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(UpdateDto.Requirements))
            {
                MessageBox.Show("Requirements is required", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }
        else
        {
            if (string.IsNullOrWhiteSpace(CreateDto.Title))
            {
                MessageBox.Show("Title is required", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(CreateDto.Location))
            {
                MessageBox.Show("Location is required", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(CreateDto.Seniority))
            {
                MessageBox.Show("Seniority is required", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(CreateDto.Company))
            {
                MessageBox.Show("Company is required", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(CreateDto.Description))
            {
                MessageBox.Show("Description is required", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(CreateDto.Requirements))
            {
                MessageBox.Show("Requirements is required", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }

        DialogResult = true;
        Close();
    }
}
