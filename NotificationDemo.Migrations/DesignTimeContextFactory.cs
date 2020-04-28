using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using NotificationDemo.DbContext;

namespace NotificationDemo.Migrations
{
    public class DesignTimeContextFactory : IDesignTimeDbContextFactory<NotificationDbContext>
    {
        public NotificationDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<NotificationDbContext>();
            builder.UseSqlServer("Server=(local);Database=sa;Trusted_Connection=True;ConnectRetryCount=0;",
                x => x.MigrationsAssembly("NotificationDemo.Migrations")
                    .MigrationsHistoryTable("VersionInfo"));

            return new NotificationDbContext(builder.Options);
        }
    }
}
