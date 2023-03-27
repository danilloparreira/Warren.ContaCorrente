using FluentMigrator.Runner.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Warren.ContaCorrente.Migrations;
using Warren.ContaCorrente.Repository;
using Warren.ContaCorrente.Repository.Abstractions;
using Warren.ContaCorrente.Services;
using Warren.ContaCorrente.Services.Abstractions;

namespace Warren.ContaCorrente.IoC
{
    public static class DependencyOfInjection
    {
        public static void ConfigureDependecies(this IServiceCollection services, IConfiguration configuration)
        {
            AddRepositories(services, configuration);
            AddServices(services);
            AddSingletons(services);
        }

        private static void AddSingletons(IServiceCollection services)
        {
            services.AddSingleton<DapperContext>();
            services.AddSingleton<Database>();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IContaCorrenteService, ContaCorrenteService>();
        }

        public static void AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MYSQL_CONTACORRENTE") ?? string.Empty;
            var provider = services.BuildServiceProvider();

            services.AddScoped<IContaCorrenteRepository, ContaCorrenteRepository>(
                r => new ContaCorrenteRepository(provider.GetRequiredService<ILogger<ContaCorrenteRepository>>(), connectionString)
            );
        }
    }
}