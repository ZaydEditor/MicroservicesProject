using OrderService.Dtos;

namespace OrderService.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllAsync();
        Task<OrderDto?> GetByIdAsync(int id);
        Task CreateAsync(CreateOrderDto createProductDto);
        Task DeleteAsync(int id);
    }
}
