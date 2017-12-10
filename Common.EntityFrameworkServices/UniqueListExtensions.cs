using System;
using System.Collections.Generic;

namespace Common.EntityFrameworkServices
{
    public static class UniqueListExtensions
    {
        public static IUniqueList<TRecord, TRecordListAssociation> WithRecords<TRecord, TRecordListAssociation>(this IUniqueList<TRecord, TRecordListAssociation> list, List<TRecord> records)
            where TRecord : class, IUniqueListRecord
            where TRecordListAssociation : IUniqueListAssociation<TRecord>
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            list.SetRecords(records);
            return list;
        }
    }
}
