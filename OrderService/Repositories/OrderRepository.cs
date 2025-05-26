using Microsoft.EntityFrameworkCore;
using OrderService.Entities;
using OrderService.Repositories.Interfaces;
using OrderService.DataAccess;

namespace OrderService.Repositories
{
    public class OrderRepository(DataContext context) : IOrderRepository
    {
        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await context.Orders.ToListAsync();
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await context.Orders.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task CreateAsync(Order order)
        {
            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Order order)
        {
            context.Orders.Remove(order);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            context.Orders.Update(order);
            await context.SaveChangesAsync();
        }
    }
}
