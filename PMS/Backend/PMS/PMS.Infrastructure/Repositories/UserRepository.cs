using Microsoft.EntityFrameworkCore;
using PMS.Application.Interfaces;
using PMS.Domain.Entities;
using ProductManagementSystem.Persistence.Context;

namespace PMS.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .Include(x => x.RefreshTokens)
            .FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
    {
        return await _context.Users
            .Include(x => x.RefreshTokens)
            .FirstOrDefaultAsync(x =>
                x.RefreshTokens.Any(r => r.Token == refreshToken));
    }

    public async Task<User> AddAsync(User user)
    {
        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        return user;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}