using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuseoData.Repositories;
using MuseoShared.DTOs;

namespace MuseoAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnnouncementsController : ControllerBase
{
    private readonly AnnouncementRepository _announcementRepository;

    public AnnouncementsController(AnnouncementRepository announcementRepository)
    {
        _announcementRepository = announcementRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetVisible()
    {
        try
        {
            var announcements = await _announcementRepository.GetVisibleAsync();

            return Ok(announcements);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "An error occurred while retrieving announcements. " + ex
            });
        }
    }

    [Authorize]
    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var announcements = await _announcementRepository.GetAllAsync();

            return Ok(announcements);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "An error occurred while retrieving announcements. " + ex
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
                    message = "The announcement id must be greater than zero."
                });
            }

            var announcement = await _announcementRepository.GetByIdAsync(id);

            if (announcement == null)
            {
                return NotFound(new
                {
                    message = "Announcement not found."
                });
            }

            return Ok(announcement);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "An error occurred while retrieving the announcement. " +ex
            });
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreateAnnouncementDto dto)
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

            if (string.IsNullOrWhiteSpace(dto.Content))
            {
                return BadRequest(new
                {
                    message = "Content is required."
                });
            }

            int newId = await _announcementRepository.CreateAsync(dto);

            return Created($"/api/announcements/{newId}", new
            {
                id = newId,
                message = "Announcement created successfully."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "An error occurred while creating the announcement. " + ex
            });
        }
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateAnnouncementDto dto)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "The announcement id must be greater than zero."
                });
            }

            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                return BadRequest(new
                {
                    message = "Title is required."
                });
            }

            if (string.IsNullOrWhiteSpace(dto.Content))
            {
                return BadRequest(new
                {
                    message = "Content is required."
                });
            }

            bool updated = await _announcementRepository.UpdateAsync(id, dto);

            if (!updated)
            {
                return NotFound(new
                {
                    message = "Announcement not found."
                });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "An error occurred while updating the announcement. " +ex
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
                    message = "The announcement id must be greater than zero."
                });
            }

            bool deleted = await _announcementRepository.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "Announcement not found."
                });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "An error occurred while deleting the announcement. " +ex
            });
        }
    }
}