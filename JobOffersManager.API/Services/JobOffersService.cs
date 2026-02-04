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

    // Get all job offers
    public List<JobOfferDto> GetAll()
        => _context.JobOffers
            .Select(j => ToDto(j))
            .ToList();

    // Get job offer by id
    public JobOfferDto? GetById(int id)
    {
        var job = _context.JobOffers.Find(id);
        return job == null ? null : ToDto(job);
    }

    // Create new job offer
    public JobOfferDto Create(CreateJobOfferDto dto)
    {
        ValidateRequiredFields(
    (dto.Title, "Title"),
    (dto.Location, "Location"),
    (dto.Seniority, "Seniority"),
    (dto.Description, "Description"),
    (dto.Requirements, "Requirements")
);

        var job = new JobOffer
        {
            Title = dto.Title,
            Seniority = dto.Seniority,
            Description = dto.Description,
            Requirements = dto.Requirements,
            Location = dto.Location,
            Company = dto.Company,
            Created = DateTime.UtcNow
        };

        _context.JobOffers.Add(job);
        _context.SaveChanges();

        return ToDto(job);
    }

    // Update existing job offer
    public JobOfferDto? Update(int id, UpdateJobOfferDto dto)
    {
            ValidateRequiredFields(
    (dto.Title, "Title"),
    (dto.Location, "Location"),
    (dto.Seniority, "Seniority"),
    (dto.Description, "Description"),
    (dto.Requirements, "Requirements"));


    var job = _context.JobOffers.Find(id);
        if (job == null) return null;

        job.Title = dto.Title;
        job.Seniority = dto.Seniority;
        job.Description = dto.Description;
        job.Requirements = dto.Requirements;
        job.Location = dto.Location;
        job.Company = dto.Company;

        _context.SaveChanges();
        return ToDto(job);
    }

    // Delete job offer by id
    public bool Delete(int id)
    {
        var job = _context.JobOffers.Find(id);
        if (job == null) return false;

        _context.JobOffers.Remove(job);
        _context.SaveChanges();
        return true;
    }

    // Map JobOffer entity to JobOfferDto
    private static JobOfferDto ToDto(JobOffer job)
        => new()
        {
            Id = job.Id,
            Title = job.Title,
            Seniority = job.Seniority,
            Description = job.Description,
            Requirements = job.Requirements,
            Location = job.Location,
            Company = job.Company,
            Created = job.Created
        };

    // Business-level validation independent of API model validation
    private static void ValidateRequiredFields(params (string Value, string Name)[] fields)
    {
        foreach (var field in fields)
        {
            if (string.IsNullOrWhiteSpace(field.Value))
                throw new ArgumentException($"{field.Name} is required");
        }
    }


}
