using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.EntityFrameworkServices
{
    public interface IUpsertMappedListService<TDbContext, TInput, TOutput>
        where TDbContext : DbContext
        where TInput : class
        where TOutput : class
    {
        Task<List<TOutput>> UpsertAsync(List<TInput> records, int parentId = 0);
    }
}
