using System.ComponentModel.DataAnnotations;

namespace JobOffersManager.Shared;

public class CreateJobOfferDto
{
    [Required]
    [MinLength(3)]
    public string Title { get; set; } = "";

    public string Seniority { get; set; } = "";

    public string Description { get; set; } = "";

    public string Requirements { get; set; } = "";
}
