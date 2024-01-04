using Microsoft.EntityFrameworkCore;

namespace Boykot.WebApp.Models
{
    public class BoykotDbContext : DbContext
    {
        public BoykotDbContext(DbContextOptions<BoykotDbContext> options):base(options)
        {
                
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Urun>()
                .Property(s => s.Aktifmi)
                .HasDefaultValueSql("CONVERT([bit],(1))");
        }
        public DbSet<Urun> Uruns { get; set; }
        public DbSet<User>  Users { get; set; }
    }
}
