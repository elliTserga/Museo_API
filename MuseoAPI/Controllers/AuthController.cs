using Microsoft.AspNetCore.Mvc;
using MuseoAuth;
using MuseoShared.DTOs;

namespace MuseoAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthenticationService _authenticationService;

    public AuthController(AuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        string? token = await _authenticationService.LoginAsync(dto);

        if (token == null)
        {
            return Unauthorized("Invalid username or password");
        }

        return Ok(new
        {
            token = token
        });
    }
}