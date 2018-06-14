using System.Collections.Generic;
using static System.Activator;

namespace Common.EntityFrameworkServices.Factories
{
    public static class UniqueListAssociationsFactory<TRecord, TRecordListAssociation>
        where TRecord : class, IUniqueListRecord
        where TRecordListAssociation : IUniqueListAssociation<TRecord>
    {
        public static List<TRecordListAssociation> Create(in List<TRecord> records)
        {
            var associations = CreateInstance<List<TRecordListAssociation>>();
            if (records == null) return associations;
            foreach (var record in records)
            {
                var association = CreateInstance<TRecordListAssociation>();
                association.SetRecord(in record);
                associations.Add(association);
            }
            return associations;
        }
    }
}
