using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuseoData.Repositories;
using MuseoShared.DTOs;
using MuseoShared.Interfaces;

namespace MuseoAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly CategoryRepository _categoryRepository;
    private readonly IStorageService _storageService;

    public CategoriesController(
        CategoryRepository categoryRepository,
        IStorageService storageService)
    {
        _categoryRepository = categoryRepository;
        _storageService = storageService;
    }


    // GET ALL CATEGORIES

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var categories =
                (await _categoryRepository.GetAllAsync()).ToList();

            var result = new List<object>();

            foreach (var category in categories)
            {
                string? imageUrl = null;

                if (!string.IsNullOrWhiteSpace(category.ImagePath))
                {
                    imageUrl =
                        await _storageService.GetFileUrlAsync(
                            category.ImagePath);
                }

                result.Add(new
                {
                    category.Id,
                    category.Name,
                    category.ImagePath,
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
                    "An error occurred while retrieving categories.",
                error = ex.Message
            });
        }
    }


    // GET CATEGORY BY ID

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
                        "The category id must be greater than zero."
                });
            }

            var category =
                await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound(new
                {
                    message = "Category not found."
                });
            }

            string? imageUrl = null;

            if (!string.IsNullOrWhiteSpace(category.ImagePath))
            {
                imageUrl =
                    await _storageService.GetFileUrlAsync(
                        category.ImagePath);
            }

            return Ok(new
            {
                category.Id,
                category.Name,
                category.ImagePath,
                ImageUrl = imageUrl
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message =
                    "An error occurred while retrieving the category.",
                error = ex.Message
            });
        }
    }


    // CREATE CATEGORY

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateCategoryDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest(new
                {
                    message =
                        "Category name is required."
                });
            }

            int newId =
                await _categoryRepository.CreateAsync(dto);

            return Created(
                $"/api/categories/{newId}",
                new
                {
                    id = newId,
                    message =
                        "Category created successfully."
                });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message =
                    "An error occurred while creating the category.",
                error = ex.Message
            });
        }
    }


    // UPDATE CATEGORY

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateCategoryDto dto)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message =
                        "The category id must be greater than zero."
                });
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest(new
                {
                    message =
                        "Category name is required."
                });
            }

            bool updated =
                await _categoryRepository.UpdateAsync(
                    id,
                    dto);

            if (!updated)
            {
                return NotFound(new
                {
                    message =
                        "Category not found."
                });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message =
                    "An error occurred while updating the category.",
                error = ex.Message
            });
        }
    }


    // UPLOAD CATEGORY IMAGE

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
                        "The category id must be greater than zero."
                });
            }

            if (file == null || file.Length == 0)
            {
                return BadRequest(new
                {
                    message =
                        "An image file is required."
                });
            }


            // ALLOWED IMAGE EXTENSIONS

            string extension =
                Path.GetExtension(file.FileName)
                    .ToLowerInvariant();

            string[] allowedExtensions =
            {
                ".jpg",
                ".jpeg",
                ".png",
                ".webp",
                ".gif"
            };


            // ALLOWED IMAGE MIME TYPES

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


            if (!allowedExtensions.Contains(extension) ||
                !allowedContentTypes.Contains(contentType))
            {
                return BadRequest(new
                {
                    message =
                        "Only valid image files are allowed. Supported formats: JPG, JPEG, PNG, WEBP, GIF, BMP and TIFF."
                });
            }


            // CHECK CATEGORY EXISTS

            var category =
                await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound(new
                {
                    message =
                        "Category not found."
                });
            }


            // CREATE MINIO PATH

            string path =
                $"categories/{id}/" +
                $"{Guid.NewGuid()}{extension}";


            // UPLOAD IMAGE TO MINIO

            await using Stream stream =
                file.OpenReadStream();

            await _storageService.UploadAsync(
                path,
                stream,
                file.ContentType,
                cancellationToken);


            // UPDATE CATEGORY IMAGE PATH

            var dto =
                new UpdateCategoryDto
                {
                    Name = category.Name,
                    ImagePath = path
                };


            bool updated =
                await _categoryRepository.UpdateAsync(
                    id,
                    dto);


            if (!updated)
            {
                return StatusCode(500, new
                {
                    message =
                        "The image was uploaded but the category could not be updated."
                });
            }


            // GENERATE TEMPORARY URL

            string imageUrl =
                await _storageService.GetFileUrlAsync(
                    path);


            return Ok(new
            {
                imagePath = path,
                imageUrl = imageUrl,
                message =
                    "Category image uploaded successfully."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message =
                    "An error occurred while uploading the category image.",
                error = ex.Message
            });
        }
    }


    // DELETE CATEGORY

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        int id,
        [FromQuery] bool force = false)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message =
                        "The category id must be greater than zero."
                });
            }

            int result =
                await _categoryRepository.DeleteAsync(
                    id,
                    force);


            if (result == -2)
            {
                return NotFound(new
                {
                    message =
                        "Category not found."
                });
            }


            if (result == -1)
            {
                return Conflict(new
                {
                    message =
                        "This category contains exhibits. Confirm the deletion to continue.",

                    warning =
                        "If the category is deleted, the associated exhibits will remain without a category.",

                    requiresConfirmation = true,

                    confirmationRequest =
                        $"/api/categories/{id}?force=true"
                });
            }


            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message =
                    "An error occurred while deleting the category.",
                error = ex.Message
            });
        }
    }
}