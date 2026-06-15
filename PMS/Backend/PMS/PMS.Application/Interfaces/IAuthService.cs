using PMS.Application.DTOs.Auth;
using System.Security.Claims;

namespace PMS.Application.Services;

public interface IAuthService
{
    Task<string> RegisterAsync(RegisterDto dto);
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
    Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
    Task LogoutAsync(string refreshToken);
    Task<CurrentUserDto> GetCurrentUserAsync(ClaimsPrincipal user);
}