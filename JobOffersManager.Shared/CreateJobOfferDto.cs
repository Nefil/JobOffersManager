using System.ComponentModel.DataAnnotations;

namespace JobOffersManager.Shared;

public class CreateJobOfferDto
{
    [Required]
    [MinLength(3)]
    public string Title { get; set; } = "";

    [Required]
    public string Seniority { get; set; } = "";
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";
    
    [Required]
    [Range(1, double.MaxValue, ErrorMessage = "Salary must be greater than 0")]
    public decimal? Salary { get; set; }
    
    [Required]
    public string Country { get; set; } = "";
    
    [Required]
    [Phone]
    public string Telephone { get; set; } = "";

    [Required]
    public string Description { get; set; } = "";

    [Required]
    public string Requirements { get; set; } = "";

    [Required]
    public string Location { get; set; } = "";

    [Required]
    public string Company { get; set; } = "";
}
