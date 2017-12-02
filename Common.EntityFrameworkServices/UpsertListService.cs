using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.EntityFrameworkServices.Services
{

    public class UpsertListService<TDbContext, TRecord> : IUpsertListService<TDbContext, TRecord>
        where TDbContext : DbContext
        where TRecord : class
    {
        private readonly ILogger<UpsertListService<TDbContext, TRecord>> _logger;
        private readonly IUpsertService<TDbContext, TRecord> _records;

        public UpsertListService(
            ILogger<UpsertListService<TDbContext, TRecord>> logger,
            IUpsertService<TDbContext, TRecord> records)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _records = records ?? throw new ArgumentNullException(nameof(records));
        }

        public async Task<List<TRecord>> UpsertAsync(List<TRecord> records)
        {
            if (records == null) return null;
            _logger.LogInformation("Upserting records");
            var list = new List<TRecord>();
            foreach (var record in records)
            {
                var saved = await _records.UpsertAsync(record);
                list.Add(saved);
            }
            return list;
        }
    }
}
