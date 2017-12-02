using Common.EntityFrameworkServices.Options;
using Common.EntityFrameworkServices.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.EntityFrameworkServices
{
    public static class AddDbConfigurationExtension
    {
        public static IServiceCollection AddInMemoryDbConfiguration<TDbContext>(this IServiceCollection serviceCollection, IConfiguration config, string databaseName)
            where TDbContext : DbContext
            => serviceCollection
                .AddDbContext<TDbContext>(opt => opt.UseInMemoryDatabase(databaseName))
                .AddRequiredServices(config);

        public static IServiceCollection AddSqlDbConfiguration<TDbContext>(this IServiceCollection serviceCollection, IConfiguration config, string connectionString)
            where TDbContext : DbContext
            => serviceCollection
                .AddDbContextPool<TDbContext>(
                    opt => opt.UseSqlServer(connectionString),
                    config.GetValue<int?>("DbConfiguration:PoolSize") ?? 128)
                .AddRequiredServices(config);

        private static IServiceCollection AddRequiredServices(this IServiceCollection serviceCollection, IConfiguration config)
            => serviceCollection
                .AddDistributedMemoryCache()
                .AddGenericServices()
                .AddLogging()
                .Configure<CacheSlidingExpiration>(
                    config.GetSection(nameof(CacheSlidingExpiration)));
    }
}
