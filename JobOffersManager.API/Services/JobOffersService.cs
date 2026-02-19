using JobOffersManager.API.Data;
using JobOffersManager.API.Entities;
using JobOffersManager.Shared;
using Microsoft.EntityFrameworkCore;

namespace JobOffersManager.API.Services;

public class JobOffersService : IJobOffersService
{
    private readonly AppDbContext _context;

    public JobOffersService(AppDbContext context)
    {
        _context = context;
    }

    // Get job offer by id
    public async Task<JobOfferDto?> GetByIdAsync(int id)
    {
        var job = await _context.JobOffers.FindAsync(id);
        return job == null ? null : ToDto(job);
    }

    // Create new job offer
    public async Task<JobOfferDto> CreateAsync(CreateJobOfferDto dto)
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

        await _context.JobOffers.AddAsync(job);
        await _context.SaveChangesAsync();

        return ToDto(job);
    }

    // Update existing job offer
    public async Task<JobOfferDto?> UpdateAsync(int id, UpdateJobOfferDto dto)
    {
        ValidateRequiredFields(
            (dto.Title, "Title"),
            (dto.Location, "Location"),
            (dto.Seniority, "Seniority"),
            (dto.Description, "Description"),
            (dto.Requirements, "Requirements")
        );

        var job = await _context.JobOffers.FindAsync(id);
        if (job == null) return null;

        job.Title = dto.Title;
        job.Seniority = dto.Seniority;
        job.Description = dto.Description;
        job.Requirements = dto.Requirements;
        job.Location = dto.Location;
        job.Company = dto.Company;

        await _context.SaveChangesAsync();
        return ToDto(job);
    }

    // Delete job offer by id
    public async Task<bool> DeleteAsync(int id)
    {
        var job = await _context.JobOffers.FindAsync(id);
        if (job == null) return false;

        _context.JobOffers.Remove(job);
        await _context.SaveChangesAsync();
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

    // Get job offers with filtering, sorting, and pagination
    public async Task<JobOffersResponseDto> GetAllAsync(JobOfferQueryDto query)
    {
        // Basic safety defaults
        if (query.Page < 1) query.Page = 1;
        if (query.PageSize < 1) query.PageSize = 10;

        var jobs = _context.JobOffers.AsQueryable();

        // Filtering by location
        if (!string.IsNullOrWhiteSpace(query.Location))
        {
            var location = query.Location.ToLower();
            jobs = jobs.Where(j =>
                EF.Functions.Like(j.Location.ToLower(), $"%{location}%"));
        }

        // Filtering by seniority
        if (!string.IsNullOrWhiteSpace(query.Seniority))
        {
            var seniority = query.Seniority.ToLower();
            jobs = jobs.Where(j =>
                EF.Functions.Like(j.Seniority.ToLower(), $"%{seniority}%"));
        }

        // Sorting
        if (!string.IsNullOrWhiteSpace(query.SortBy))
        {
            var isDesc = query.SortOrder?.ToLower() == "desc";

            jobs = query.SortBy.ToLower() switch
            {
                "title" => isDesc
                    ? jobs.OrderByDescending(j => j.Title)
                    : jobs.OrderBy(j => j.Title),

                "created" => isDesc
                    ? jobs.OrderByDescending(j => j.Created)
                    : jobs.OrderBy(j => j.Created),

                _ => jobs.OrderByDescending(j => j.Created)
            };
        }

        // Total count BEFORE pagination
        var totalCount = await jobs.CountAsync();

        // Pagination
        var items = await jobs
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(j => ToDto(j))
            .ToListAsync();

        // Final response
        return new JobOffersResponseDto
        {
            Items = items,
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }
}

