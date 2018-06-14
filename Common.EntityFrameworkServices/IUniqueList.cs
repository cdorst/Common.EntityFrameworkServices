using System.Collections.Generic;

namespace Common.EntityFrameworkServices
{
    public interface IUniqueList<TRecord, TRecordListAssociation>
        where TRecord : class, IUniqueListRecord
        where TRecordListAssociation : IUniqueListAssociation<TRecord>
    {
        List<TRecordListAssociation> GetAssociations();
        void SetRecords(in List<TRecord> records);
    }
}
