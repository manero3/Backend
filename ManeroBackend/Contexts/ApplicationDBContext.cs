using Microsoft.EntityFrameworkCore;

namespace ManeroBackend.Contexts
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext()
        {
                
        }

        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
        }
    }
}
