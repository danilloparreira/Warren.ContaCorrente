using FluentMigrator.Runner;
using FluentMigrator.Runner.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Warren.ContaCorrente.Migrations
{
    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var migrationService = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

                try
                {
                    migrationService.ListMigrations();
                    migrationService.MigrateDown(3);
                    migrationService.MigrateUp();
                }
                catch
                {
                    Console.WriteLine("Ocorreu um erro na migração da base!");
                    throw;
                }
            }

            return host;
        }
    }
}
