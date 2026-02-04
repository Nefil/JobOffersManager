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
    public string Description { get; set; } = "";

    [Required]
    public string Requirements { get; set; } = "";

    [Required]
    public string Location { get; set; } = "";

    [Required]
    public string Company { get; set; } = "";

}
