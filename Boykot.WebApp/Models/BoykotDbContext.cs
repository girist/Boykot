using Microsoft.EntityFrameworkCore;

namespace Boykot.WebApp.Models
{
    public class BoykotDbContext : DbContext
    {
        public BoykotDbContext(DbContextOptions<BoykotDbContext> options):base(options)
        {
                
        }

        public DbSet<Urun> Uruns { get; set; }
        public DbSet<User>  Users { get; set; }
    }
}
