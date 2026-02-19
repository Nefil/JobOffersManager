using Microsoft.AspNetCore.Mvc;
using JobOffersManager.API.Services;
using JobOffersManager.Shared;

namespace JobOffersManager.API.Controllers;

[ApiController]
[Route("api/jobs")]
public class JobsController : ControllerBase
{
    private readonly IJobOffersService _service;

    public JobsController(IJobOffersService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] JobOfferQueryDto query)
    {
        return Ok(await _service.GetAllAsync(query));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var job = await _service.GetByIdAsync(id);
        return job == null ? NotFound() : Ok(job);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateJobOfferDto dto)
    {
        return Ok(await _service.CreateAsync(dto));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateJobOfferDto dto)
    {
        var job = await _service.UpdateAsync(id, dto);
        return job == null ? NotFound() : Ok(job);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        return await _service.DeleteAsync(id)
            ? NoContent()
            : NotFound();
    }
}

