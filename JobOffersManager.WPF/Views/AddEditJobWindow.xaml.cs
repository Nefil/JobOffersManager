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
        var ok = IsEditMode 
            ? ValidateForm(UpdateDto.Title, UpdateDto.Location, UpdateDto.Seniority, 
                          UpdateDto.Company, UpdateDto.Description, UpdateDto.Requirements,
                          UpdateDto.Country, UpdateDto.Email, UpdateDto.Telephone, UpdateDto.Salary)
            : ValidateForm(CreateDto.Title, CreateDto.Location, CreateDto.Seniority,
                          CreateDto.Company, CreateDto.Description, CreateDto.Requirements,
                          CreateDto.Country, CreateDto.Email, CreateDto.Telephone, CreateDto.Salary);

        if (!ok) return;

        DialogResult = true;
        Close();
    }

    private bool ValidateForm(string title, string location, string seniority, 
                             string company, string description, string requirements,
                             string country, string email, string telephone, decimal? salary)
    {
        if (string.IsNullOrWhiteSpace(title))
            return ShowError("Title is required");

        if (string.IsNullOrWhiteSpace(location))
            return ShowError("Location is required");

        if (string.IsNullOrWhiteSpace(seniority))
            return ShowError("Seniority is required");

        if (string.IsNullOrWhiteSpace(company))
            return ShowError("Company is required");

        if (string.IsNullOrWhiteSpace(description))
            return ShowError("Description is required");

        if (string.IsNullOrWhiteSpace(requirements))
            return ShowError("Requirements is required");

        if (string.IsNullOrWhiteSpace(country))
            return ShowError("Country is required");

        if (string.IsNullOrWhiteSpace(email))
            return ShowError("Email is required");

        if (string.IsNullOrWhiteSpace(telephone))
            return ShowError("Telephone is required");

        if (!salary.HasValue || salary <= 0)
            return ShowError("Salary is required and must be greater than 0");

        return true;
    }

    private bool ShowError(string message)
    {
        MessageBox.Show(message, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return false;
    }
}
