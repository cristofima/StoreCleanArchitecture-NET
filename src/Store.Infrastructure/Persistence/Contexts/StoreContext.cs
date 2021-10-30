using Microsoft.EntityFrameworkCore;
using Store.ApplicationCore.Entities;

namespace Store.Infrastructure.Persistence.Contexts
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}