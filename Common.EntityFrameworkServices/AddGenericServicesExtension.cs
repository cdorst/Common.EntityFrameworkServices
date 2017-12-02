using Microsoft.Extensions.DependencyInjection;

namespace Common.EntityFrameworkServices.Services
{
    public static class AddGenericServicesExtension
    {
        public static IServiceCollection AddGenericServices(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddScoped(typeof(ICacheService<>), typeof(CacheService<>))
                .AddScoped(typeof(ICacheWithDefaultKeysService<>), typeof(CacheWithDefaultKeysService<>))
                .AddScoped(typeof(IEntityKeyValuesService<>), typeof(EntityKeyValuesService<>))
                .AddScoped(typeof(IRepository<,>), typeof(CachedRepository<,>))
                .AddScoped(typeof(IUpsertListService<,>), typeof(UpsertListService<,>))
                .AddScoped(typeof(IUpsertMappedListService<,,>), typeof(UpsertMappedListService<,,>))
                .AddScoped(typeof(IUpsertUniqueListService<,,,>), typeof(UpsertUniqueListService<,,,>));
    }
}
