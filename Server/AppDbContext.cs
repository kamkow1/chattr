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

        public DbSet<Message> Messages { get; set; } 

        public DbSet<GroupChat> GroupChats { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<NotificationType> NotificationTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();
        }
    }
}