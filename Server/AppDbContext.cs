using chattr.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace chattr.Server
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
                
        }

        public DbSet<User> Users { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();
        }
    }
}