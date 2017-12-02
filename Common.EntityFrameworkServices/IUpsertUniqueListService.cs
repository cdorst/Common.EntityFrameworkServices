using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Common.EntityFrameworkServices.Services
{
    public interface IUpsertUniqueListService<TDbContext, TRecord, TRecordList, TRecordListAssociation>
        where TDbContext : DbContext
        where TRecord : class, IUniqueListRecord
        where TRecordList : class, IUniqueList<TRecord, TRecordListAssociation>
        where TRecordListAssociation : IUniqueListAssociation<TRecord>
    {
        Task<TRecordList> UpsertAsync(TRecordList recordList);
    }
}