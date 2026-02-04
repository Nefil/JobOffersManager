namespace JobOffersManager.Shared;

public class JobOfferQueryDto
{
    public string? Location { get; set; }
    public string? Seniority { get; set; }

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
