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
    public string Email { get; set; } = "";
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Salary must be greater than 0")]
    public int? Salary { get; set; }
    [Required]
    public string Country { get; set; } = "";
    [Required]
    [Range(100000000, 999999999, ErrorMessage = "Telephone must be a valid 9-digit number")]
    public int? Telephone { get; set; }

    [Required]
    public string Description { get; set; } = "";

    [Required]
    public string Requirements { get; set; } = "";

    [Required]
    public string Location { get; set; } = "";

    [Required]
    public string Company { get; set; } = "";
}
