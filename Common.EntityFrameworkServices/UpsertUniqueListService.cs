using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Common.EntityFrameworkServices.Services
{
    public class UpsertUniqueListService<TDbContext, TRecord, TRecordList, TRecordListAssociation> : IUpsertUniqueListService<TDbContext, TRecord, TRecordList, TRecordListAssociation>
        where TDbContext : DbContext
        where TRecord : class, IUniqueListRecord
        where TRecordList : class, IUniqueList<TRecord, TRecordListAssociation>
        where TRecordListAssociation : IUniqueListAssociation<TRecord>
    {
        private readonly ILogger<UpsertUniqueListService<TDbContext, TRecord, TRecordList, TRecordListAssociation>> _logger;
        private readonly IUpsertListService<TDbContext, TRecord> _records;
        private readonly IUpsertService<TDbContext, TRecordList> _recordList;

        public UpsertUniqueListService(
            ILogger<UpsertUniqueListService<TDbContext, TRecord, TRecordList, TRecordListAssociation>> logger,
            IUpsertListService<TDbContext, TRecord> records,
            IUpsertService<TDbContext, TRecordList> recordList)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _records = records ?? throw new ArgumentNullException(nameof(records));
            _recordList = recordList ?? throw new ArgumentNullException(nameof(recordList));
        }

        public async Task<TRecordList> UpsertAsync(TRecordList recordList)
        {
            if (recordList == null) return null;
            _logger.LogInformation("Getting child records to upsert from list entity");
            var records = recordList.GetAssociations()?.Select(a => a.GetRecord()).ToList();
            if (records == null || records.Count == 0) return null;
            _logger.LogInformation("Saving child records prior to saving list entity");
            records = await _records.UpsertAsync(records);
            _logger.LogInformation("Setting saved records on list entity");
            recordList.SetRecords(records);
            _logger.LogInformation("Upserting record list");
            return await _recordList.UpsertAsync(recordList);
        }
    }
}
