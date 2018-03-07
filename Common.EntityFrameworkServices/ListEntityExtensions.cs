using System.Collections.Generic;
using System.Linq;
using static System.Activator;

namespace Common.EntityFrameworkServices
{
    public static class ListEntityExtensions
    {
        public static IEnumerable<TRecord> GetRecords<TRecord, TRecordListAssociation>(
            this IUniqueList<TRecord, TRecordListAssociation> list)
            where TRecordListAssociation : IUniqueListAssociation<TRecord>
            where TRecord : class, IUniqueListRecord
            => list.GetAssociations()?.Select(each => each.GetRecord());

        public static TRecordList Merge<TRecordList, TRecord, TRecordListAssociation>(
            this TRecordList existing,
            TRecordList given,
            IEqualityComparer<TRecord> equalityComparer)
            where TRecordList : class, IUniqueList<TRecord, TRecordListAssociation>
            where TRecord : class, IUniqueListRecord
            where TRecordListAssociation : IUniqueListAssociation<TRecord>
        {
            var instance = CreateInstance<TRecordList>();
            instance.SetRecords(
                (existing?.GetRecords() ?? new List<TRecord>())
                .Union(given.GetRecords(), equalityComparer)
                .ToList());
            return instance;
        }
    }
}
