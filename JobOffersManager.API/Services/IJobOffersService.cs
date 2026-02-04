using JobOffersManager.Shared;

namespace JobOffersManager.API.Services;

public interface IJobOffersService
{
    List<JobOfferDto> GetAll();
    List<JobOfferDto> GetAll(JobOfferQueryDto query);
    JobOfferDto? GetById(int id);
    JobOfferDto Create(CreateJobOfferDto dto);
    JobOfferDto? Update(int id, UpdateJobOfferDto dto);
    bool Delete(int id);
}
