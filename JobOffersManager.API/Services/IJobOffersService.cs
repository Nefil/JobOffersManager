using JobOffersManager.Shared;

namespace JobOffersManager.API.Services;

public interface IJobOffersService
{
    Task<JobOfferDto?> GetByIdAsync(int id);
    Task<JobOfferDto> CreateAsync(CreateJobOfferDto dto);
    Task<JobOfferDto?> UpdateAsync(int id, UpdateJobOfferDto dto);
    Task<bool> DeleteAsync(int id);
    Task<JobOffersResponseDto> GetAllAsync(JobOfferQueryDto query);
}
