using PMS.Domain.Entities;

namespace PMS.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);

    Task<User?> GetByRefreshTokenAsync(string refreshToken);

    Task<User> AddAsync(User user);

    Task SaveChangesAsync();
}