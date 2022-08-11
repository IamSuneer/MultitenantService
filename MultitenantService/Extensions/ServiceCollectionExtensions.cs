using Microsoft.EntityFrameworkCore;
using MultitenantService.Data;
using MultitenantService.Options;

namespace MultitenantService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAndMigrateTenantDatabases(this IServiceCollection services, IConfiguration config)
        {
            var options = services.GetOptions<TenantSetting>(nameof(TenantSetting));
            var defaultConnectionString = options.Defaults?.ConnectionString;
            var defaultDbProvider = options.Defaults?.DBProvider;
            if (defaultDbProvider.ToLower() == "sharedTenant")
            {
                services.AddDbContext<MultitenantDbContext>(m => m.UseSqlServer(e => e.MigrationsAssembly(typeof(MultitenantDbContext).Assembly.FullName)));
            }
            var tenants = options.Tenants;
            foreach (var tenant in tenants)
            {
                string connectionString;
                if (string.IsNullOrEmpty(tenant.ConnectionString))
                {
                    connectionString = defaultConnectionString;
                }
                else
                {
                    connectionString = tenant.ConnectionString;
                }
                using var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<MultitenantDbContext>();
                dbContext.Database.SetConnectionString(connectionString);
                if (dbContext.Database.GetMigrations().Count() > 0)
                {
                    dbContext.Database.Migrate();
                }
            }
            return services;
        }
        public static T GetOptions<T>(this IServiceCollection services, string sectionName) where T : new()
        {
            using var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var section = configuration.GetSection(sectionName);
            var options = new T();
            section.Bind(options);
            return options;
        }
    }
}
