using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuseoData.Repositories;
using MuseoShared.DTOs;

namespace MuseoAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MuseumController : ControllerBase
{
    private readonly MuseumRepository _museumRepository;

    public MuseumController(MuseumRepository museumRepository)
    {
        _museumRepository = museumRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var museum = await _museumRepository.GetAsync();

            if (museum == null)
            {
                return NotFound(new
                {
                    message = "Museum not found."
                });
            }

            return Ok(museum);
        }
        catch (Exception)
        {
            return StatusCode(500, new
            {
                message = "An error occurred while retrieving the museum."
            });
        }
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> Update(UpdateMuseumDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest(new
                {
                    message = "Museum name is required."
                });
            }

            if (string.IsNullOrWhiteSpace(dto.Location))
            {
                return BadRequest(new
                {
                    message = "Location is required."
                });
            }

            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                return BadRequest(new
                {
                    message = "Email is required."
                });
            }

            bool updated = await _museumRepository.UpdateAsync(dto);

            if (!updated)
            {
                return NotFound(new
                {
                    message = "Museum not found."
                });
            }

            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(500, new
            {
                message = "An error occurred while updating the museum."
            });
        }
    }
}