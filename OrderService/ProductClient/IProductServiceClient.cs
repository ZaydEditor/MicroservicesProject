using OrderService.Dtos;

namespace OrderService.ProductClient
{
    public interface IProductServiceClient
    {
        Task<ProductDto?> GetProductAsync(int productId);
        Task<bool> UpdateStockAsync(int productId, int quantityChange);
    }
}
