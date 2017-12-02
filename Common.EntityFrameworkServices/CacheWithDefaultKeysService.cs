using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Common.EntityFrameworkServices.Services
{
    public class CacheWithDefaultKeysService<TRecord> : ICacheWithDefaultKeysService<TRecord>
        where TRecord : class
    {
        private readonly ICacheService<TRecord> _cache;
        private readonly IEntityKeyValuesService<TRecord> _keyValues;
        private readonly ILogger<CacheWithDefaultKeysService<TRecord>> _logger;

        public CacheWithDefaultKeysService(
            ICacheService<TRecord> cache,
            IEntityKeyValuesService<TRecord> keyValues,
            ILogger<CacheWithDefaultKeysService<TRecord>> logger)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _keyValues = keyValues ?? throw new ArgumentNullException(nameof(keyValues));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TRecord> FindAsync(params object[] keyValues)
            => await _cache.FindAsync(_keyValues.GetCacheKey(keyValues));

        public async Task RemoveAsync(params object[] keyValues)
            => await _cache.RemoveAsync(_keyValues.GetCacheKey(keyValues));

        public async Task SaveAsync(TRecord record)
            => await _cache.SaveAsync(_keyValues.GetCacheKey(record), record);
    }
}
