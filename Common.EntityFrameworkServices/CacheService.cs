using Common.EntityFrameworkServices.Options;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProtoBuf;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Common.EntityFrameworkServices.Services
{
    public class CacheService<TRecord> : ICacheService<TRecord>
        where TRecord : class
    {
        private readonly IDistributedCache _cache;
        private readonly DistributedCacheEntryOptions _expiration;
        private readonly ILogger<CacheService<TRecord>> _logger;

        public CacheService(
            IDistributedCache cache,
            ILogger<CacheService<TRecord>> logger,
            IOptions<CacheSlidingExpiration> options)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            var expiration = options?.Value ?? new CacheSlidingExpiration();
            _expiration = new DistributedCacheEntryOptions
            {
                SlidingExpiration = new TimeSpan(
                    expiration.Days ?? 0,
                    expiration.Hours ?? 0,
                    expiration.Minutes ?? 0,
                    expiration.Seconds ?? 0)
            };
        }

        public async Task<TRecord> FindAsync(string key)
        {
            _logger.LogInformation($"Find entry: {key}");
            var cacheEntry = await _cache.GetAsync(key);
            if (cacheEntry != null)
            {
                _logger.LogInformation("Cache hit");
                return Serializer.Deserialize<TRecord>(new MemoryStream(cacheEntry));
            }
            _logger.LogInformation("Cache miss");
            return null;
        }

        public async Task RemoveAsync(string key)
        {
            _logger.LogInformation($"Removing entry: {key}");
            await _cache.RemoveAsync(key);
        }

        public async Task SaveAsync(string key, TRecord record)
        {
            _logger.LogInformation($"Setting entry: {key}");
            var value = Serialize(record);
            await _cache.SetAsync(key, value, _expiration);
        }

        private byte[] Serialize(TRecord record)
        {
            _logger.LogInformation("ProtoBuf serializing record");
            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, record);
                return stream.ToArray();
            }
        }
    }
}
