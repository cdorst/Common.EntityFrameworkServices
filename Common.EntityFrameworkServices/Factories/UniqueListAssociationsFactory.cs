using System;
using System.Collections.Generic;

namespace Common.EntityFrameworkServices.Factories
{
    public static class UniqueListAssociationsFactory<TRecord, TRecordListAssociation>
        where TRecord : class, IUniqueListRecord
        where TRecordListAssociation : IUniqueListAssociation<TRecord>
    {
        public static List<TRecordListAssociation> Create(List<TRecord> records)
        {
            var associations = Activator.CreateInstance<List<TRecordListAssociation>>();
            if (records == null) return associations;
            foreach (var record in records)
            {
                var association = Activator.CreateInstance<TRecordListAssociation>();
                association.SetRecord(record);
                associations.Add(association);
            }
            return associations;
        }
    }
}
