using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuseoShared.DTOs;
using MuseoData.Repositories;

namespace MuseoAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExhibitsController : ControllerBase
{
    private readonly ExhibitRepository _exhibitRepository;

    public ExhibitsController(ExhibitRepository exhibitRepository)
    {
        _exhibitRepository = exhibitRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var exhibits = await _exhibitRepository.GetAllAsync();

        return Ok(exhibits);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var exhibit = await _exhibitRepository.GetByIdAsync(id);

        if (exhibit == null)
        {
            return NotFound();
        }

        return Ok(exhibit);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreateExhibitDto dto)
    {
        int newId = await _exhibitRepository.CreateAsync(dto);

        return Created($"/api/exhibits/{newId}", new
        {
            Id = newId,
            Message = "Exhibit created successfully"
        });
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateExhibitDto dto)
    {
        bool updated = await _exhibitRepository.UpdateAsync(id, dto);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        bool deleted = await _exhibitRepository.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}