using System.Reflection;
using Microsoft.EntityFrameworkCore;
using NotificationDemo.Domain;
using NotificationDemo.Mapping.Config;

namespace NotificationDemo.DbContext
{
    public class NotificationDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public NotificationDbContext(DbContextOptions contextOptions) : base(contextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.AddEntityConfigurationsFromAssembly(
                typeof(EntityMappingConfiguration<>).GetTypeInfo().Assembly);
        }

        public DbSet<Subscription> Subscriptions { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
