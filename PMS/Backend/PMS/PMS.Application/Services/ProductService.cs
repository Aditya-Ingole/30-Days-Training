using AutoMapper;
using PMS.Application.DTOs.Product;
using PMS.Application.Exceptions;
using PMS.Application.Interfaces;
using PMS.Application.Services;
using PMS.Domain.Entities;
using Serilog;

namespace PMS.Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public ProductService(
        IProductRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<ProductResponseDto>> GetAllAsync()
    {
        var products = await _repository.GetAllAsync();

        return _mapper.Map<List<ProductResponseDto>>(products);
    }

    public async Task<ProductResponseDto> GetByIdAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);

        if (product == null)
        {
            throw new NotFoundException($"Product with id {id} was not found.");
        }

        return _mapper.Map<ProductResponseDto>(product);
    }

    public async Task<ProductResponseDto> CreateAsync(CreateProductDto dto)
    {
        Log.Information("Creating product {ProductName}", dto.Name);

        var product = _mapper.Map<Product>(dto);

        await _repository.AddAsync(product);

        Log.Information("Product created successfully with Id {ProductId}", product.Id);

        return _mapper.Map<ProductResponseDto>(product);
    }

    public async Task<ProductResponseDto> UpdateAsync(int id, UpdateProductDto dto)
    {
        Log.Information("Updating product with Id {ProductId}", id);
        var product = await _repository.GetByIdAsync(id);

        if (product == null)
        {
            throw new NotFoundException($"Product with id {id} was not found.");
        }

        _mapper.Map(dto, product);

        await _repository.UpdateAsync(product);

        return _mapper.Map<ProductResponseDto>(product);
    }

    public async Task DeleteAsync(int id)
    {
        Log.Information("Deleting product with Id {ProductId}", id);
        var product = await _repository.GetByIdAsync(id);

        if (product == null)
        {
            throw new NotFoundException($"Product with id {id} was not found.");
        }

        await _repository.DeleteAsync(product);
    }
}