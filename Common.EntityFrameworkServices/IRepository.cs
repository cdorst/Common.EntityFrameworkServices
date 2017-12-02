using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Common.EntityFrameworkServices.Services
{
    public interface IRepository<TDbContext, TEntity>
        where TDbContext : DbContext
        where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> FindAsync(params object[] keyValues);
        Task RemoveAsync(params object[] keyValues);
        Task<TEntity> UpdateAsync(TEntity entity);
    }
}
