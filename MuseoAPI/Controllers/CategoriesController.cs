using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuseoData.Repositories;
using MuseoShared.DTOs;

namespace MuseoAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly CategoryRepository _categoryRepository;

    public CategoriesController(CategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var categories = await _categoryRepository.GetAllAsync();

            return Ok(categories);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "An error occurred while retrieving categories.",
                error = ex.Message
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
                    message = "The category id must be greater than zero."
                });
            }

            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound(new
                {
                    message = "Category not found."
                });
            }

            return Ok(category);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "An error occurred while retrieving the category.",
                error = ex.Message
            });
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest(new
                {
                    message = "Category name is required."
                });
            }

            int newId = await _categoryRepository.CreateAsync(dto);

            return Created($"/api/categories/{newId}", new
            {
                id = newId,
                message = "Category created successfully."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "An error occurred while creating the category.",
                error = ex.Message
            });
        }
    }

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
                    message = "The category id must be greater than zero."
                });
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest(new
                {
                    message = "Category name is required."
                });
            }

            bool updated = await _categoryRepository.UpdateAsync(id, dto);

            if (!updated)
            {
                return NotFound(new
                {
                    message = "Category not found."
                });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "An error occurred while updating the category.",
                error = ex.Message
            });
        }
    }

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
                    message = "The category id must be greater than zero."
                });
            }

            int result = await _categoryRepository.DeleteAsync(id, force);

            if (result == -2)
            {
                return NotFound(new
                {
                    message = "Category not found."
                });
            }

            if (result == -1)
            {
                return Conflict(new
                {
                    message = "This category contains exhibits. Confirm the deletion to continue.",
                    warning = "If the category is deleted, the associated exhibits will remain without a category.",
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
                message = "An error occurred while deleting the category.",
                error = ex.Message
            });
        }
    }
}