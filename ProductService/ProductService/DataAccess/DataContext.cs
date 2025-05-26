using Microsoft.EntityFrameworkCore;
using ProductService.Entities;

namespace ProductService.DataAccess
{
    public class DataContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }

    }
}
