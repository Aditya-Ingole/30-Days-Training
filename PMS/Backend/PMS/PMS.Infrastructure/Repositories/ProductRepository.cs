using Microsoft.EntityFrameworkCore;
using PMS.Application.Interfaces;
using PMS.Domain.Entities;
using ProductManagementSystem.Persistence.Context;

namespace PMS.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Product> AddAsync(Product product)
        {
            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            return product;
        }

        public async Task DeleteAsync(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

       
    }
}
