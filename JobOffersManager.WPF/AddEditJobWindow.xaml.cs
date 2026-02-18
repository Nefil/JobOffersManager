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
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
        Close();
    }
}
