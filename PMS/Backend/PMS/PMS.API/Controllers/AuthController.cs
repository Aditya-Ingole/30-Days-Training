using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS.Application.DTOs.Auth;
using PMS.Application.Services;

namespace PMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var message = await _authService.RegisterAsync(dto);
        return Ok(new { message });
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
    {
        var response = await _authService.LoginAsync(dto);
        return Ok(response);
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<AuthResponseDto>> RefreshToken(RefreshTokenDto dto)
    {
        var response =
            await _authService
                .RefreshTokenAsync(dto.RefreshToken);

        return Ok(response);
    }


    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<CurrentUserDto>> GetCurrentUser()
    {
        var user = await _authService.GetCurrentUserAsync(User);

        return Ok(user);
    }



    [HttpPost("logout")]
    public async Task<IActionResult> Logout(LogoutDto dto)
    {
        await _authService.LogoutAsync(dto.RefreshToken);

        return Ok(new
        {
            message = "Logged out successfully."
        });
    }
}