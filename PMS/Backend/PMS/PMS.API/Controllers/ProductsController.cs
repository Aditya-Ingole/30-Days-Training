using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS.Application.DTOs.Product;
using PMS.Application.Services;

namespace PMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<ProductResponseDto>>> GetAll()
    {
        var products = await _productService.GetAllAsync();

        return Ok(products);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductResponseDto>> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);

        return Ok(product);
    }


    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<ProductResponseDto>> Create(
        CreateProductDto dto)
    {
        var product = await _productService.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = product.Id },
            product);
    }


    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult<ProductResponseDto>> Update(
        int id,
        UpdateProductDto dto)
    {
        var product = await _productService.UpdateAsync(id, dto);

        return Ok(product);
    }


    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _productService.DeleteAsync(id);

        return NoContent();
    }
}