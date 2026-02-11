using JobOffersManager.Shared;

public class JobOffersResponseDto
{
    public List<JobOfferDto> Items { get; set; } = new();
    public int TotalCount { get; set; }

    public int Page { get; set; }
    public int PageSize { get; set; }

    public int TotalPages =>
        PageSize == 0 ? 0 : (int)Math.Ceiling((double)TotalCount / PageSize);
}
