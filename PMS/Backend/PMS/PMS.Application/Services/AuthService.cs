using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PMS.Application.DTOs.Auth;
using PMS.Application.Interfaces;
using PMS.Application.Services;
using PMS.Domain.Entities;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Claims;


namespace PMS.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly PasswordHasher<User> _passwordHasher;

    public AuthService(
        IUserRepository userRepository,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<string> RegisterAsync(RegisterDto dto)
    {
        Log.Information("User registration started for {Email}", dto.Email);

        var existingUser = await _userRepository.GetByEmailAsync(dto.Email);

        if (existingUser != null)
        {
            throw new ArgumentException("Email is already registered.");
        }

        var user = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Role = "User"
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

        await _userRepository.AddAsync(user);

        return "User registered successfully.";
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);

        if (user == null)
        {
            throw new ArgumentException(
                "Invalid email or password.");
        }

        var verificationResult =
            _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                dto.Password);

        if (verificationResult ==
            PasswordVerificationResult.Failed)
        {
            throw new ArgumentException(
                "Invalid email or password.");
        }

        var token = GenerateJwtToken(user);
        Log.Information("User {Email} logged in successfully", user.Email);

        var refreshToken = GenerateRefreshToken();

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            Expires = DateTime.UtcNow.AddDays(7),
            IsRevoked = false
        });

        await _userRepository.SaveChangesAsync();

        return new AuthResponseDto
        {
            Token = token,
            RefreshToken = refreshToken
        };
    }

    private string GenerateRefreshToken()
    {
        return Guid.NewGuid().ToString();
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("Jwt");

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings["Key"]!));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(jwtSettings["DurationInMinutes"])),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(
     string refreshToken)
    {
        var user =
            await _userRepository
                .GetByRefreshTokenAsync(refreshToken);

        if (user == null)
        {
            throw new ArgumentException(
                "Invalid refresh token.");
        }

        var existingToken =
            user.RefreshTokens
                .FirstOrDefault(x =>
                    x.Token == refreshToken);

        if (existingToken == null)
        {
            throw new ArgumentException(
                "Refresh token not found.");
        }

        if (existingToken.IsRevoked)
        {
            throw new ArgumentException(
                "Refresh token revoked.");
        }

        if (existingToken.Expires < DateTime.UtcNow)
        {
            throw new ArgumentException(
                "Refresh token expired.");
        }

        existingToken.IsRevoked = true;

        var newJwtToken =
            GenerateJwtToken(user);

        var newRefreshToken =
            GenerateRefreshToken();

        user.RefreshTokens.Add(
            new RefreshToken
            {
                Token = newRefreshToken,
                Expires = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            });

        await _userRepository.SaveChangesAsync();

        return new AuthResponseDto
        {
            Token = newJwtToken,
            RefreshToken = newRefreshToken
        };
    }

    public async Task<CurrentUserDto> GetCurrentUserAsync(ClaimsPrincipal user)
    {
        var email = user.FindFirst(ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentException("User email not found.");
        }

        var currentUser = await _userRepository.GetByEmailAsync(email);

        if (currentUser == null)
        {
            throw new ArgumentException("User not found.");
        }

        return new CurrentUserDto
        {
            Id = currentUser.Id,
            Name = $"{currentUser.FirstName} {currentUser.LastName}",
            Email = currentUser.Email,
            Role = currentUser.Role
        };
    }

    public async Task LogoutAsync(string refreshToken)
    {
        var user =
            await _userRepository
                .GetByRefreshTokenAsync(refreshToken);

        if (user == null)
        {
            throw new ArgumentException(
                "Invalid refresh token.");
        }

        var token =
            user.RefreshTokens
                .FirstOrDefault(x =>
                    x.Token == refreshToken);

        if (token == null)
        {
            throw new ArgumentException(
                "Refresh token not found.");
        }

        token.IsRevoked = true;

        await _userRepository.SaveChangesAsync();

        Log.Information(
            "User {Email} logged out successfully",
            user.Email);
    }
}