using System;
using System.Collections.Generic;

namespace Common.EntityFrameworkServices.Factories
{
    public static class UniqueListFactory<TRecord, TRecordList, TRecordListAssociation>
        where TRecord : class, IUniqueListRecord
        where TRecordList : class, IUniqueList<TRecord, TRecordListAssociation>
        where TRecordListAssociation : IUniqueListAssociation<TRecord>
    {
        public static TRecordList Create(List<TRecord> records)
        {
            var instance = Activator.CreateInstance<TRecordList>();
            instance.SetRecords(records);
            return instance;
        }
    }
}
