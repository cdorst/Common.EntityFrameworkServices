using static System.Activator;
using System.Collections.Generic;

namespace Common.EntityFrameworkServices.Factories
{
    public static class UniqueListFactory<TRecord, TRecordList, TRecordListAssociation>
        where TRecord : class, IUniqueListRecord
        where TRecordList : class, IUniqueList<TRecord, TRecordListAssociation>
        where TRecordListAssociation : IUniqueListAssociation<TRecord>
    {
        public static TRecordList Create(in List<TRecord> records)
        {
            var instance = CreateInstance<TRecordList>();
            instance.SetRecords(in records);
            return instance;
        }
    }
}
