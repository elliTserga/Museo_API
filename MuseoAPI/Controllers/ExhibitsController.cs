using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuseoData.Repositories;
using MuseoShared.DTOs;
using MuseoShared.Interfaces;

namespace MuseoAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExhibitsController : ControllerBase
{
    private readonly ExhibitRepository _exhibitRepository;
    private readonly IStorageService _storageService;

    public ExhibitsController(
        ExhibitRepository exhibitRepository,
        IStorageService storageService)
    {
        _exhibitRepository = exhibitRepository;
        _storageService = storageService;
    }


    // PUBLIC - VISIBLE EXHIBITS ONLY

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var exhibits =
                (await _exhibitRepository.GetAllAsync())
                .ToList();

            var result = new List<object>();

            foreach (var exhibit in exhibits)
            {
                string? imageUrl = null;

                if (!string.IsNullOrWhiteSpace(
                    exhibit.ImagePath))
                {
                    imageUrl =
                        await _storageService
                            .GetFileUrlAsync(
                                exhibit.ImagePath);
                }

                result.Add(new
                {
                    exhibit.Id,
                    exhibit.Title,
                    exhibit.Description,
                    exhibit.Year,
                    exhibit.CategoryId,
                    exhibit.Visible,
                    exhibit.ImagePath,
                    ImageUrl = imageUrl
                });
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message =
                    "An error occurred while retrieving exhibits.",
                error = ex.Message
            });
        }
    }


    // ADMIN - ALL EXHIBITS

    [Authorize]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllForAdmin()
    {
        try
        {
            var exhibits =
                (await _exhibitRepository
                    .GetAllForAdminAsync())
                .ToList();

            var result = new List<object>();

            foreach (var exhibit in exhibits)
            {
                string? imageUrl = null;

                if (!string.IsNullOrWhiteSpace(
                    exhibit.ImagePath))
                {
                    imageUrl =
                        await _storageService
                            .GetFileUrlAsync(
                                exhibit.ImagePath);
                }

                result.Add(new
                {
                    exhibit.Id,
                    exhibit.Title,
                    exhibit.Description,
                    exhibit.Year,
                    exhibit.CategoryId,
                    exhibit.Visible,
                    exhibit.ImagePath,
                    ImageUrl = imageUrl
                });
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message =
                    "An error occurred while retrieving all exhibits.",
                error = ex.Message
            });
        }
    }


    // GET BY ID

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message =
                        "The exhibit id must be greater than zero."
                });
            }

            var exhibit =
                await _exhibitRepository
                    .GetDetailsByIdAsync(id);

            if (exhibit == null)
            {
                return NotFound(new
                {
                    message =
                        "Exhibit not found."
                });
            }

            if (!string.IsNullOrWhiteSpace(
                exhibit.ImagePath))
            {
                exhibit.ImageUrl =
                    await _storageService
                        .GetFileUrlAsync(
                            exhibit.ImagePath);
            }

            return Ok(exhibit);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message =
                    "An error occurred while retrieving the exhibit.",
                error = ex.Message
            });
        }
    }


    // GET BY CATEGORY

    [HttpGet("category/{categoryId}")]
    public async Task<IActionResult> GetByCategoryId(
        int categoryId)
    {
        try
        {
            if (categoryId <= 0)
            {
                return BadRequest(new
                {
                    message =
                        "The category id must be greater than zero."
                });
            }

            var exhibits =
                (await _exhibitRepository
                    .GetByCategoryIdAsync(categoryId))
                .ToList();

            var result = new List<object>();

            foreach (var exhibit in exhibits)
            {
                string? imageUrl = null;

                if (!string.IsNullOrWhiteSpace(
                    exhibit.ImagePath))
                {
                    imageUrl =
                        await _storageService
                            .GetFileUrlAsync(
                                exhibit.ImagePath);
                }

                result.Add(new
                {
                    exhibit.Id,
                    exhibit.Title,
                    exhibit.Description,
                    exhibit.Year,
                    exhibit.CategoryId,
                    exhibit.Visible,
                    exhibit.ImagePath,
                    ImageUrl = imageUrl
                });
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message =
                    "An error occurred while retrieving exhibits by category.",
                error = ex.Message
            });
        }
    }


    // CREATE

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateExhibitDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(
                dto.Title))
            {
                return BadRequest(new
                {
                    message =
                        "Title is required."
                });
            }

            if (dto.CategoryId <= 0)
            {
                return BadRequest(new
                {
                    message =
                        "A valid category id is required."
                });
            }

            int newId =
                await _exhibitRepository
                    .CreateAsync(dto);

            return Created(
                $"/api/exhibits/{newId}",
                new
                {
                    id = newId,
                    message =
                        "Exhibit created successfully."
                });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message =
                    "An error occurred while creating the exhibit.",
                error = ex.Message
            });
        }
    }


    // UPDATE

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
                    message =
                        "The exhibit id must be greater than zero."
                });
            }

            if (string.IsNullOrWhiteSpace(
                dto.Title))
            {
                return BadRequest(new
                {
                    message =
                        "Title is required."
                });
            }

            if (dto.CategoryId <= 0)
            {
                return BadRequest(new
                {
                    message =
                        "A valid category id is required."
                });
            }

            bool updated =
                await _exhibitRepository
                    .UpdateAsync(id, dto);

            if (!updated)
            {
                return NotFound(new
                {
                    message =
                        "Exhibit not found."
                });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message =
                    "An error occurred while updating the exhibit.",
                error = ex.Message
            });
        }
    }


    // UPLOAD COVER IMAGE

    [Authorize]
    [HttpPost("{id}/image")]
    public async Task<IActionResult> UploadImage(
        int id,
        [FromForm] IFormFile file,
        CancellationToken cancellationToken)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message =
                        "The exhibit id must be greater than zero."
                });
            }

            if (file == null ||
                file.Length == 0)
            {
                return BadRequest(new
                {
                    message =
                        "An image file is required."
                });
            }


            string extension =
                Path.GetExtension(
                    file.FileName)
                .ToLowerInvariant();


            string[] allowedExtensions =
            {
                ".jpg",
                ".jpeg",
                ".png",
                ".webp",
                ".gif"
            };


            string[] allowedContentTypes =
            {
                "image/jpeg",
                "image/png",
                "image/webp",
                "image/gif"
            };


            string contentType =
                file.ContentType
                    .ToLowerInvariant();


            if (!allowedExtensions.Contains(
                    extension) ||
                !allowedContentTypes.Contains(
                    contentType))
            {
                return BadRequest(new
                {
                    message =
                        "Only valid image files are allowed. Supported formats: JPG, JPEG, PNG, WEBP, GIF, BMP and TIFF."
                });
            }


            var exhibit =
                await _exhibitRepository
                    .GetByIdAsync(id);

            if (exhibit == null)
            {
                return NotFound(new
                {
                    message =
                        "Exhibit not found."
                });
            }


            string path =
                $"exhibits/{id}/cover/" +
                $"{Guid.NewGuid()}{extension}";


            await using Stream stream =
                file.OpenReadStream();


            await _storageService.UploadAsync(
                path,
                stream,
                file.ContentType,
                cancellationToken);


            var dto =
                new UpdateExhibitDto
                {
                    Title =
                        exhibit.Title,

                    Description =
                        exhibit.Description,

                    Year =
                        exhibit.Year,

                    CategoryId =
                        exhibit.CategoryId ?? 0,

                    Visible =
                        exhibit.Visible,

                    ImagePath =
                        path
                };


            bool updated =
                await _exhibitRepository
                    .UpdateAsync(id, dto);


            if (!updated)
            {
                return StatusCode(500, new
                {
                    message =
                        "The image was uploaded but the exhibit could not be updated."
                });
            }


            string imageUrl =
                await _storageService
                    .GetFileUrlAsync(path);


            return Ok(new
            {
                imagePath =
                    path,

                imageUrl =
                    imageUrl,

                message =
                    "Exhibit image uploaded successfully."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message =
                    "An error occurred while uploading the exhibit image.",
                error =
                    ex.Message
            });
        }
    }


    // DELETE

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        int id)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message =
                        "The exhibit id must be greater than zero."
                });
            }

            bool deleted =
                await _exhibitRepository
                    .DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message =
                        "Exhibit not found."
                });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message =
                    "An error occurred while deleting the exhibit.",
                error =
                    ex.Message
            });
        }
    }
}