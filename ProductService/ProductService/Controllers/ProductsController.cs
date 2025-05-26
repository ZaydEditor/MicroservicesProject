using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Dtos;
using ProductService.Services.Interfaces;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductService productService) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var product = await productService.GetByIdAsync(id);
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromServices] IValidator<CreateProductDto> validator,
            [FromBody] CreateProductDto createProductDto)
        {
            var result = await validator.ValidateAsync(createProductDto);
            if (!result.IsValid)
                return BadRequest(result.Errors);

            await productService.CreateAsync(createProductDto);
            return Created();
        }

        [HttpPut("{id}/stock")]
        public async Task<IActionResult> UpdateStock([FromRoute] int id, [FromBody] UpdateStockDto updateStockDto)
        {
            await productService.UpdateStockAsync(id, updateStockDto);
            return NoContent();
        }
    }
}
