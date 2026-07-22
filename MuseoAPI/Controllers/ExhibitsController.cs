using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuseoData.Repositories;
using MuseoShared.DTOs;

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
        try
        {
            var exhibits = await _exhibitRepository.GetAllAsync();

            return Ok(exhibits);
        }
        catch (Exception)
        {
            return StatusCode(500, new
            {
                message = "An error occurred while retrieving exhibits."
            });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "The exhibit id must be greater than zero."
                });
            }

            var exhibit = await _exhibitRepository.GetDetailsByIdAsync(id);

            if (exhibit == null)
            {
                return NotFound(new
                {
                    message = "Exhibit not found."
                });
            }

            return Ok(exhibit);
        }
        catch (Exception)
        {
            return StatusCode(500, new
            {
                message = "An error occurred while retrieving the exhibit."
            });
        }
    }

    [HttpGet("category/{categoryId}")]
    public async Task<IActionResult> GetByCategoryId(int categoryId)
    {
        try
        {
            if (categoryId <= 0)
            {
                return BadRequest(new
                {
                    message = "The category id must be greater than zero."
                });
            }

            var exhibits =
                await _exhibitRepository.GetByCategoryIdAsync(categoryId);

            return Ok(exhibits);
        }
        catch (Exception)
        {
            return StatusCode(500, new
            {
                message = "An error occurred while retrieving exhibits by category."
            });
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreateExhibitDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                return BadRequest(new
                {
                    message = "Title is required."
                });
            }

            if (dto.CategoryId <= 0)
            {
                return BadRequest(new
                {
                    message = "A valid category id is required."
                });
            }

            int newId = await _exhibitRepository.CreateAsync(dto);

            return Created($"/api/exhibits/{newId}", new
            {
                id = newId,
                message = "Exhibit created successfully."
            });
        }
        catch (Exception)
        {
            return StatusCode(500, new
            {
                message = "An error occurred while creating the exhibit."
            });
        }
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateExhibitDto dto)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "The exhibit id must be greater than zero."
                });
            }

            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                return BadRequest(new
                {
                    message = "Title is required."
                });
            }

            if (dto.CategoryId <= 0)
            {
                return BadRequest(new
                {
                    message = "A valid category id is required."
                });
            }

            bool updated =
                await _exhibitRepository.UpdateAsync(id, dto);

            if (!updated)
            {
                return NotFound(new
                {
                    message = "Exhibit not found."
                });
            }

            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(500, new
            {
                message = "An error occurred while updating the exhibit."
            });
        }
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "The exhibit id must be greater than zero."
                });
            }

            bool deleted =
                await _exhibitRepository.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "Exhibit not found."
                });
            }

            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(500, new
            {
                message = "An error occurred while deleting the exhibit."
            });
        }
    }
}