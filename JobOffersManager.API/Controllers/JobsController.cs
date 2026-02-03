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
    public IActionResult GetAll()
        => Ok(_service.GetAll());

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var job = _service.GetById(id);
        return job == null ? NotFound() : Ok(job);
    }

    [HttpPost]
    public IActionResult Create(CreateJobOfferDto dto)
        => Ok(_service.Create(dto));

    [HttpPut("{id}")]
    public IActionResult Update(int id, UpdateJobOfferDto dto)
    {
        var job = _service.Update(id, dto);
        return job == null ? NotFound() : Ok(job);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
        => _service.Delete(id) ? NoContent() : NotFound();
}
