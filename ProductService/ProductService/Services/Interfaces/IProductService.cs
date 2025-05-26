using ProductService.Dtos;
using ProductService.Entities;

namespace ProductService.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task CreateAsync(CreateProductDto createProductDto);
        Task UpdateStockAsync(int id, UpdateStockDto updateStockDto);
        Task DeleteAsync(int id);
    }
}
