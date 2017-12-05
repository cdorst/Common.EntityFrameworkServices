using System.Threading.Tasks;

namespace Common.EntityFrameworkServices
{
    public interface ICacheService<TRecord>
        where TRecord : class
    {
        Task<TRecord> FindAsync(string key);
        Task RemoveAsync(string key);
        Task SaveAsync(string key, TRecord record);
    }
}
