using Microsoft.EntityFrameworkCore;

namespace ManeroBackend.Contexts
{
    public class ProductContext : DbContext
    {
        public ProductContext()
        {

        }
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {
        }

        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
