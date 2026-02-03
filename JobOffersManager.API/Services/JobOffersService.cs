using JobOffersManager.Shared;

namespace JobOffersManager.API.Services;

public class JobOffersService : IJobOffersService
{
    private static readonly List<JobOfferDto> _jobs = new();
    private static int _nextId = 1;

    public List<JobOfferDto> GetAll() => _jobs;

    public JobOfferDto? GetById(int id)
        => _jobs.FirstOrDefault(j => j.Id == id);

    public JobOfferDto Create(CreateJobOfferDto dto)
    {
        var job = new JobOfferDto
        {
            Id = _nextId++,
            Title = dto.Title,
            Seniority = dto.Seniority,
            Description = dto.Description,
            Requirements = dto.Requirements,
            Created = DateTime.UtcNow
        };

        _jobs.Add(job);
        return job;
    }

    public JobOfferDto? Update(int id, UpdateJobOfferDto dto)
    {
        var job = GetById(id);
        if (job == null) return null;

        job.Title = dto.Title;
        job.Seniority = dto.Seniority;
        job.Description = dto.Description;
        job.Requirements = dto.Requirements;

        return job;
    }

    public bool Delete(int id)
    {
        var job = GetById(id);
        if (job == null) return false;

        _jobs.Remove(job);
        return true;
    }
}
