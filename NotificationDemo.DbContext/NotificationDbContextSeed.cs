using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using NotificationDemo.Domain;

namespace NotificationDemo.DbContext
{
    public static class NotificationDbContextSeed
    {
        public static void EnsureSeedInitialData(this NotificationDbContext context)
        {
            if (context.InitialMigrationApplied())
            {
                context.FillUsers();

                context.SaveChanges();

                using var connection = context.Database.GetDbConnection();
                connection.Open();

                using var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO \"VersionInfo\" (\"MigrationId\", \"ProductVersion\") VALUES(@p0, @p1);";
                command.Parameters.Add(
                    new SqlParameter("p0", SqlDbType.NVarChar).SetValue(SeedInitialDataMigrationId));
                command.Parameters.Add(
                    new SqlParameter("p1", SqlDbType.NVarChar).SetValue("-"));
                command.ExecuteNonQuery();
            }
        }

        public static bool InitialMigrationApplied(this Microsoft.EntityFrameworkCore.DbContext context)
        {
            var allAppliedMigrations = context.GetService<IHistoryRepository>()
                .GetAppliedMigrations()
                .Select(x => x.MigrationId)
                .ToArray();

            return allAppliedMigrations.Any(x => x.EndsWith(InitialMigrationId)) &&
                   !allAppliedMigrations.Contains(SeedInitialDataMigrationId);
        }

        #region private

        private static SqlParameter SetValue(this SqlParameter sqlParameter, object value)
        {
            sqlParameter.Value = value;
            return sqlParameter;
        }

        private static void FillUsers(this NotificationDbContext context)
        {
            for (var i = 1; i <= 5; i++)
            {
                context.Users.Add(new User
                {
                    Login = $"user{i}",
                    Name = $"user{i}"
                });
            }
        }

        private const string InitialMigrationId = "InitialMigration";
        private const string SeedInitialDataMigrationId = "SeedInitialDataMigration";

        #endregion private
    }
}
