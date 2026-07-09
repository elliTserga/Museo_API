using Microsoft.AspNetCore.Mvc;
using MuseoData.Repositories;

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
        var museum = await _museumRepository.GetAsync();

        if (museum == null)
        {
            return NotFound();
        }

        return Ok(museum);
    }
}