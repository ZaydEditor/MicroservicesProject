using AutoMapper;
using OrderService.Dtos;
using OrderService.Entities;
using OrderService.Exceptions;
using OrderService.ProductClient;
using OrderService.Repositories.Interfaces;
using OrderService.Services.Interfaces;

namespace OrderService.Services
{
    public class OrderServiceImpl(IOrderRepository orderRepo, IMapper mapper, IProductServiceClient productServiceClient) : IOrderService
    {
        public async Task<IEnumerable<OrderDto>> GetAllAsync()
        {
            var orders = await orderRepo.GetAllAsync();
            if (orders == null || !orders.Any()) 
                throw new NotFoundException("No orders found");

            return mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto?> GetByIdAsync(int id)
        {
            var order = await orderRepo.GetByIdAsync(id);
            if (order == null) throw new NotFoundException("Order not found");

            return mapper.Map<OrderDto>(order);
        }

        public async Task CreateAsync(CreateOrderDto createOrderDto)
        {
            var product = await productServiceClient.GetProductAsync(createOrderDto.ProductId);

            if (product == null)
            {
                throw new NotFoundException("Product not found");
            }

            if (product.Stock < createOrderDto.Quantity)
            {
                throw new Exception("Not enough stock available");
            }

            var stockUpdated = await productServiceClient.UpdateStockAsync(createOrderDto.ProductId, createOrderDto.Quantity);
            if (!stockUpdated)
            {
                throw new Exception("Failed to update product stock");
            }

            var order = mapper.Map<Order>(createOrderDto);
            order.TotalPrice = product.Price * order.Quantity;

            await orderRepo.CreateAsync(order);
        }

        public async Task DeleteAsync(int id)
        {
            var order = await orderRepo.GetByIdAsync(id);
            if (order == null) throw new NotFoundException("Order not found");

            await orderRepo.DeleteAsync(order);
        }

    }
}
