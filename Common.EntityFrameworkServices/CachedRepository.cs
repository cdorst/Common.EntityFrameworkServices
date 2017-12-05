using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Common.EntityFrameworkServices
{
    public class CachedRepository<TDbContext, TEntity> : Repository<TDbContext, TEntity>
        where TDbContext : DbContext
        where TEntity : class
    {
        private readonly ICacheWithDefaultKeysService<TEntity> _cache;

        public CachedRepository(
            ICacheWithDefaultKeysService<TEntity> cache,
            TDbContext context,
            ILogger<Repository<TDbContext, TEntity>> logger) : base(context, logger)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public override async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null) return null;
            _logger.LogInformation("Adding record to database");
            entity = await base.AddAsync(entity);
            await SaveCacheEntry(entity);
            return entity;
        }

        public override async Task<TEntity> FindAsync(params object[] keyValues)
        {
            if ((keyValues?.Length ?? 0) == 0) return null;
            _logger.LogInformation("Finding record from cache");
            var cached = await _cache.FindAsync(keyValues);
            if (cached != null) return cached;
            _logger.LogInformation("Finding record from database");
            var entity = await base.FindAsync(keyValues);
            if (entity != null) await SaveCacheEntry(entity);
            return entity;
        }

        public override async Task RemoveAsync(params object[] keyValues)
        {
            if ((keyValues?.Length ?? 0) == 0) return;
            _logger.LogInformation("Removing record from cache");
            await _cache.RemoveAsync(keyValues);
            _logger.LogInformation("Removing record from database");
            await base.RemoveAsync(keyValues);
        }

        public override async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity == null) return null;
            _logger.LogInformation("Saving record in database");
            entity = await base.UpdateAsync(entity);
            _logger.LogInformation("Saving record in cache");
            await _cache.SaveAsync(entity);
            return entity;
        }

        private async Task SaveCacheEntry(TEntity entity)
        {
            _logger.LogInformation("Saving cache entry");
            await _cache.SaveAsync(entity);
        }
    }
}
