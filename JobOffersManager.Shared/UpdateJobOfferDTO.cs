using System.ComponentModel.DataAnnotations;

namespace JobOffersManager.Shared;

public class UpdateJobOfferDto
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
}
