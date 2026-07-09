using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuseoShared.DTOs;
using MuseoData.Repositories;

namespace MuseoAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MediaController : ControllerBase
{
    private readonly MediaRepository _mediaRepository;

    public MediaController(MediaRepository mediaRepository)
    {
        _mediaRepository = mediaRepository;
    }

    [HttpGet("exhibit/{exhibitId}")]
    public async Task<IActionResult> GetByExhibitId(int exhibitId)
    {
        var mediaItems = await _mediaRepository.GetByExhibitIdAsync(exhibitId);

        return Ok(mediaItems);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreateMediaItemDto dto)
    {
        int newId = await _mediaRepository.CreateAsync(dto);

        return Created($"/api/media/{newId}", new
        {
            Id = newId,
            Message = "Media item created successfully"
        });
    }
}