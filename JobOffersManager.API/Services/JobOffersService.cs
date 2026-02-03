using JobOffersManager.API.Data;
using JobOffersManager.API.Entities;
using JobOffersManager.Shared;

namespace JobOffersManager.API.Services;

public class JobOffersService : IJobOffersService
{
    private readonly AppDbContext _context;

    public JobOffersService(AppDbContext context)
    {
        _context = context;
    }

    public List<JobOfferDto> GetAll()
        => _context.JobOffers
            .Select(j => ToDto(j))
            .ToList();

    public JobOfferDto? GetById(int id)
    {
        var job = _context.JobOffers.Find(id);
        return job == null ? null : ToDto(job);
    }

    public JobOfferDto Create(CreateJobOfferDto dto)
    {
        var job = new JobOffer
        {
            Title = dto.Title,
            Seniority = dto.Seniority,
            Description = dto.Description,
            Requirements = dto.Requirements,
            Created = DateTime.UtcNow
        };

        _context.JobOffers.Add(job);
        _context.SaveChanges();

        return ToDto(job);
    }

    public JobOfferDto? Update(int id, UpdateJobOfferDto dto)
    {
        var job = _context.JobOffers.Find(id);
        if (job == null) return null;

        job.Title = dto.Title;
        job.Seniority = dto.Seniority;
        job.Description = dto.Description;
        job.Requirements = dto.Requirements;

        _context.SaveChanges();
        return ToDto(job);
    }

    public bool Delete(int id)
    {
        var job = _context.JobOffers.Find(id);
        if (job == null) return false;

        _context.JobOffers.Remove(job);
        _context.SaveChanges();
        return true;
    }

    private static JobOfferDto ToDto(JobOffer job)
        => new()
        {
            Id = job.Id,
            Title = job.Title,
            Seniority = job.Seniority,
            Description = job.Description,
            Requirements = job.Requirements,
            Created = job.Created
        };
}
