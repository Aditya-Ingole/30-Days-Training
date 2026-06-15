using Microsoft.EntityFrameworkCore;
using PMS.Domain.Entities;


namespace ProductManagementSystem.Persistence.Context;

public class AppDbContext : DbContext
{
    // Constructor for runtime (when app is running)
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // Parameterless constructor for migrations (when creating database)
    public AppDbContext()
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Name = "Laptop",
                Description = "Dell Laptop",
                Price = 50000
            },
            new Product
            {
                Id = 2,
                Name = "Mobile",
                Description = "Samsung Mobile",
                Price = 25000
            }
        );

        modelBuilder.Entity<User>().HasData(
    new User
    {
        Id = 1,
        FirstName = "Admin",
        LastName = "User",
        Email = "admin@gmail.com",
        PasswordHash = "Admin123",
        Role = "Admin"
    }
);
    }
}