using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.EntityFrameworkServices.Services
{
    public interface IUpsertListService<TDbContext, TRecord>
        where TDbContext : DbContext
        where TRecord : class
    {
        Task<List<TRecord>> UpsertAsync(List<TRecord> records);
    }
}
