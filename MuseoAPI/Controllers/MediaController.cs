using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuseoAPI.Requests;
using MuseoData.Repositories;
using MuseoShared.DTOs;
using MuseoShared.Interfaces;

namespace MuseoAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MediaController : ControllerBase
{
    private readonly MediaRepository _mediaRepository;
    private readonly IStorageService _storageService;

    public MediaController(
        MediaRepository mediaRepository,
        IStorageService storageService)
    {
        _mediaRepository = mediaRepository;
        _storageService = storageService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var mediaItems = await _mediaRepository.GetAllAsync();

            return Ok(mediaItems);
        }
        catch (Exception)
        {
            return StatusCode(500, new
            {
                message = "An error occurred while retrieving media items."
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
                    message = "The media item id must be greater than zero."
                });
            }

            var mediaItem = await _mediaRepository.GetByIdAsync(id);

            if (mediaItem == null)
            {
                return NotFound(new
                {
                    message = "Media item not found."
                });
            }

            return Ok(mediaItem);
        }
        catch (Exception)
        {
            return StatusCode(500, new
            {
                message = "An error occurred while retrieving the media item."
            });
        }
    }

    [HttpGet("exhibit/{exhibitId}")]
    public async Task<IActionResult> GetByExhibitId(int exhibitId)
    {
        try
        {
            if (exhibitId <= 0)
            {
                return BadRequest(new
                {
                    message = "The exhibit id must be greater than zero."
                });
            }

            var mediaItems =
                await _mediaRepository.GetByExhibitIdAsync(exhibitId);

            return Ok(mediaItems);
        }
        catch (Exception)
        {
            return StatusCode(500, new
            {
                message = "An error occurred while retrieving media items for the exhibit."
            });
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromForm] CreateMediaItemRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (request.ExhibitId <= 0)
            {
                return BadRequest(new
                {
                    message = "A valid exhibit id is required."
                });
            }

            if (request.File == null || request.File.Length == 0)
            {
                return BadRequest(new
                {
                    message = "A file is required."
                });
            }

            string extension = Path.GetExtension(request.File.FileName);

            string path =
                $"exhibits/{request.ExhibitId}/{Guid.NewGuid()}{extension}";

            await using Stream stream = request.File.OpenReadStream();

            await _storageService.UploadAsync(
                path,
                stream,
                request.File.ContentType,
                cancellationToken);

            var dto = new CreateMediaItemDto
            {
                ExhibitId = request.ExhibitId,
                FileName = request.File.FileName,
                FileType = request.File.ContentType,
                Url = path,
                Size = request.File.Length
            };

            int newId = await _mediaRepository.CreateAsync(dto);

            return Created($"/api/media/{newId}", new
            {
                id = newId,
                path,
                message = "Media item uploaded successfully."
            });
        }
        catch (Exception)
        {
            return StatusCode(500, new
            {
                message = "An error occurred while uploading the media item."
            });
        }
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateMediaItemDto dto)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "The media item id must be greater than zero."
                });
            }

            if (dto.ExhibitId <= 0)
            {
                return BadRequest(new
                {
                    message = "A valid exhibit id is required."
                });
            }

            if (string.IsNullOrWhiteSpace(dto.FileName))
            {
                return BadRequest(new
                {
                    message = "File name is required."
                });
            }

            if (string.IsNullOrWhiteSpace(dto.FileType))
            {
                return BadRequest(new
                {
                    message = "File type is required."
                });
            }

            if (string.IsNullOrWhiteSpace(dto.Url))
            {
                return BadRequest(new
                {
                    message = "URL is required."
                });
            }

            bool updated = await _mediaRepository.UpdateAsync(id, dto);

            if (!updated)
            {
                return NotFound(new
                {
                    message = "Media item not found."
                });
            }

            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(500, new
            {
                message = "An error occurred while updating the media item."
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
                    message = "The media item id must be greater than zero."
                });
            }

            var mediaItem = await _mediaRepository.GetByIdAsync(id);

            if (mediaItem == null)
            {
                return NotFound(new
                {
                    message = "Media item not found."
                });
            }

            await _storageService.DeleteAsync(mediaItem.Url);

            bool deleted = await _mediaRepository.DeleteAsync(id);

            if (!deleted)
            {
                return StatusCode(500, new
                {
                    message = "Failed to delete media item."
                });
            }

            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(500, new
            {
                message = "An error occurred while deleting the media item."
            });
        }
    }
}