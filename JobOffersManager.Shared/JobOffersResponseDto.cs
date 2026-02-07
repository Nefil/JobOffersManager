
namespace JobOffersManager.Shared;

public class JobOffersResponseDto
{
    public List<JobOfferDto> Items { get; set; } = [];
    public int TotalCount { get; set; }
}
