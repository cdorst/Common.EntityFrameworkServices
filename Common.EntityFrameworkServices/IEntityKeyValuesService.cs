using System.Collections.Generic;

namespace Common.EntityFrameworkServices
{
    public interface IEntityKeyValuesService<TEntity>
        where TEntity : class
    {
        string GetCacheKey(in TEntity entity);
        string GetCacheKey(in IEnumerable<object> keyValues);
        IEnumerable<object> GetKeyValues(TEntity entity);
    }
}
