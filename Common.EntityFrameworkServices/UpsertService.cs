using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Common.EntityFrameworkServices
{
    public class UpsertService<TDbContext, TRecord> : IUpsertService<TDbContext, TRecord>
        where TDbContext : DbContext
        where TRecord : class
    {
        protected readonly TDbContext _database;
        protected readonly ILogger<UpsertService<TDbContext, TRecord>> _logger;

        private readonly ICacheService<TRecord> _cache;
        private readonly DbSet<TRecord> _records;

        public UpsertService(
            ICacheService<TRecord> cache,
            TDbContext database,
            ILogger<UpsertService<TDbContext, TRecord>> logger,
            DbSet<TRecord> records)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _records = records ?? throw new ArgumentNullException(nameof(records));
        }

        protected virtual Action<TRecord, TRecord> AssignChanges { get; } = (existing, given) => { };
        protected Func<TRecord, string> CacheKey { get; set; }
        protected bool TrackingEnabled { get; set; }

        private bool CacheHit { get; set; }

        public virtual async Task<TRecord> UpsertAsync(TRecord given)
        {
            if (given == null) return null;
            _logger.LogInformation("Upserting references");
            given = await UpsertReferences(given);
            _logger.LogInformation("Upserting record & detaching");
            var result = await UpsertRecord(given);
            _database.Entry(result).State = EntityState.Detached;
            _logger.LogInformation("Assigning computed properties");
            result = await AssignComputedProperties(result);
            _logger.LogInformation("Assigning upserted dependents");
            result = await AssignUpsertedDependents(result);
            _logger.LogInformation("Setting cache entry");
            await _cache.SaveAsync(CacheKey(result), result);
            return result;
        }

        protected virtual Task<TRecord> AssignComputedProperties(TRecord record) => Task.FromResult(record);
        protected virtual Task<TRecord> AssignUpsertedDependents(TRecord record) => Task.FromResult(record);
        protected virtual Task<TRecord> AssignUpsertedReferences(TRecord record) => Task.FromResult(record);
        protected virtual IEnumerable<object> EnumerateReferences(TRecord record) => null;
        protected virtual Expression<Func<TRecord, bool>> FindExisting(TRecord record) => existing => false;

        private async Task<TRecord> AddNewRecord(TRecord record)
        {
            _logger.LogInformation("Adding new record");
            _records.Add(record);
            return await SaveAndDetachReferences(record);
        }

        private async Task<TRecord> FindExistingRecord(TRecord record)
        {
            if (TrackingEnabled)
            {
                _logger.LogInformation("Find database record with tracking enabled");
                return await _records.FirstOrDefaultAsync(FindExisting(record));
            }
            _logger.LogInformation("Find in cache");
            var cacheEntry = await _cache.FindAsync(CacheKey(record));
            CacheHit = cacheEntry != null;
            _logger.LogInformation($"Cache hit: {CacheHit}");
            if (CacheHit) return cacheEntry;
            _logger.LogInformation("Find in database");
            return await _records.AsNoTracking().FirstOrDefaultAsync(FindExisting(record));
        }

        private async Task<TRecord> SaveAndDetachReferences(TRecord record)
        {
            _logger.LogInformation("Saving changes");
            await _database.SaveChangesAsync();
            SetReferenceState(record, EntityState.Detached);
            return record;
        }

        private void SetReferenceState(TRecord record, EntityState state)
        {
            _logger.LogInformation($"Setting reference state: {state}");
            foreach (var reference in EnumerateReferences(record) ?? new object[] { })
                if (reference != null)
                    _database.Entry(reference).State = state;
        }

        private async Task<TRecord> UpsertRecord(TRecord given)
        {
            _logger.LogInformation("Upserting given record");
            var existing = await FindExistingRecord(given);
            if (existing == null) return await AddNewRecord(given);
            AssignChanges(existing, given);
            return (CacheHit) ? existing : await SaveAndDetachReferences(existing);
        }

        private async Task<TRecord> UpsertReferences(TRecord record)
        {
            _logger.LogInformation("Upserting references");
            record = await AssignUpsertedReferences(record);
            SetReferenceState(record, EntityState.Unchanged);
            return record;
        }
    }
}
