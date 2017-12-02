using System.Collections.Generic;

namespace Common.EntityFrameworkServices.Services
{
    public interface IEntityKeyValuesService<TEntity>
        where TEntity : class
    {
        string GetCacheKey(TEntity entity);
        string GetCacheKey(IEnumerable<object> keyValues);
        IEnumerable<object> GetKeyValues(TEntity entity);
    }
}
