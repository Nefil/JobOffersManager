using JobOffersManager.Shared;
using System.Windows;

namespace JobOffersManager.WPF;

public partial class AddEditJobWindow : Window
{
    public CreateJobOfferDto CreateDto { get; private set; } = null!;
    public UpdateJobOfferDto UpdateDto { get; private set; } = null!;

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
            Email = existingJob.Email,
            Salary = existingJob.Salary,
            Country = existingJob.Country,
            Telephone = existingJob.Telephone,
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

            if (string.IsNullOrWhiteSpace(UpdateDto.Country))
            {
                MessageBox.Show("Country is required", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(UpdateDto.Email))
            {
                MessageBox.Show("Email is required", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(UpdateDto.Telephone))
            {
                MessageBox.Show("Telephone is required", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!UpdateDto.Salary.HasValue || UpdateDto.Salary <= 0)
            {
                MessageBox.Show("Salary is required and must be greater than 0", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
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

            if (string.IsNullOrWhiteSpace(CreateDto.Country))
            {
                MessageBox.Show("Country is required", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(CreateDto.Email))
            {
                MessageBox.Show("Email is required", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(CreateDto.Telephone))
            {
                MessageBox.Show("Telephone is required", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!CreateDto.Salary.HasValue || CreateDto.Salary <= 0)
            {
                MessageBox.Show("Salary is required and must be greater than 0", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }

        DialogResult = true;
        Close();
    }
}
