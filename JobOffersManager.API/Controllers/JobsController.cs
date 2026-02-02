using Microsoft.AspNetCore.Mvc;
using JobOffersManager.Shared;

namespace JobOffersManager.API.Controllers;

[ApiController]
[Route("api/jobs")]
public class JobsController : ControllerBase
{
    private static readonly List<JobOfferDto> _jobs = new();
    private static int _nextId = 1;

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_jobs);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var job = _jobs.FirstOrDefault(j => j.Id == id);

        if (job == null)
            return NotFound();

        return Ok(job);
    }

    [HttpPost]
    public IActionResult Create(CreateJobOfferDto dto)
    {
        var job = new JobOfferDto
        {
            Id = _nextId++,
            Title = dto.Title,
            Seniority = dto.Seniority
        };

        _jobs.Add(job);

        return Ok(job);
    }
}
