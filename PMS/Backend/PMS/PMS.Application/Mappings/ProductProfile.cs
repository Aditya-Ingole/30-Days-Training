using AutoMapper;
using PMS.Application.DTOs.Product;
using PMS.Domain.Entities;

namespace PMS.Application.Mappings;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductResponseDto>();

        CreateMap<CreateProductDto, Product>();

        CreateMap<UpdateProductDto, Product>();
    }
}