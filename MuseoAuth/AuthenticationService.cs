using MuseoData.Repositories;
using MuseoShared.DTOs;

namespace MuseoAuth;

public class AuthenticationService
{
    private readonly AuthRepository _authRepository;
    private readonly JwtTokenService _jwtTokenService;
    private readonly PasswordService _passwordService;

    public AuthenticationService(
    AuthRepository authRepository,
    JwtTokenService jwtTokenService,
    PasswordService passwordService)
    {
        _authRepository = authRepository;
        _jwtTokenService = jwtTokenService;
        _passwordService = passwordService;
    }

    public async Task<string?> LoginAsync(LoginDto dto)
    {
        var user = await _authRepository.GetUserAsync(dto.Username);

        if (user == null)
        {
            return null;
        }

        if (!_passwordService.VerifyPassword(dto.Password, user.PasswordHash))
        {
            return null;
        }

        return _jwtTokenService.GenerateToken(user);
    }
}