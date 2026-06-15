using PMS.Application.DTOs.Product;

namespace PMS.Application.Services;

public interface IProductService
{
    Task<List<ProductResponseDto>> GetAllAsync();

    Task<ProductResponseDto> GetByIdAsync(int id);

    Task<ProductResponseDto> CreateAsync(CreateProductDto dto);

    Task<ProductResponseDto> UpdateAsync(int id, UpdateProductDto dto);

    Task DeleteAsync(int id);
}