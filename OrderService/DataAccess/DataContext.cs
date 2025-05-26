using Microsoft.EntityFrameworkCore;
using OrderService.Entities;

namespace OrderService.DataAccess
{
    public class DataContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Order> Orders { get; set; }
    }

}
