using Microsoft.AspNetCore.Mvc;
using JobOffersManager.API.Services;
using JobOffersManager.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace JobOffersManager.API.Controllers;

[ApiController]
[Route("api/jobs")]
public class JobsController : ControllerBase
{
    private readonly IJobOffersService _service;
    private readonly ILogger<JobsController> _logger;

    public JobsController(IJobOffersService service, ILogger<JobsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll([FromQuery] JobOfferQueryDto query)
    {
        return Ok(await _service.GetAllAsync(query));
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var job = await _service.GetByIdAsync(id);
        return job == null ? NotFound() : Ok(job);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateJobOfferDto dto)
    {
        var user = User.Identity?.Name;
        var roles = User.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.Role)
                               .Select(c => c.Value);
        
        _logger.LogInformation($"Create called by user: {user}, roles: {string.Join(",", roles)}");
        
        return Ok(await _service.CreateAsync(dto));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, UpdateJobOfferDto dto)
    {
        var user = User.Identity?.Name;
        var roles = User.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.Role)
                               .Select(c => c.Value);
        
        _logger.LogInformation($"Update called by user: {user}, roles: {string.Join(",", roles)}, id: {id}");
        
        var job = await _service.UpdateAsync(id, dto);
        return job == null ? NotFound() : Ok(job);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        return await _service.DeleteAsync(id)
            ? NoContent()
            : NotFound();
    }
}

