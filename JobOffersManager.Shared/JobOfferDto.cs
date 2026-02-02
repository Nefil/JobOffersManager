namespace JobOffersManager.Shared;

public class JobOfferDto
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Seniority { get; set; } = "";
    public string Description { get; set; } = "";
    public string Requirements { get; set; } = "";
    public DateTime Created { get; set; } = DateTime.UtcNow;
}
