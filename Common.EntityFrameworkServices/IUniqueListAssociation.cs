namespace Common.EntityFrameworkServices
{
    public interface IUniqueListAssociation<TRecord>
        where TRecord : class, IUniqueListRecord
    {
        TRecord GetRecord();
        void SetRecord(in TRecord record);
    }
}
