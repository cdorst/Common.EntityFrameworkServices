using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Common.EntityFrameworkServices
{
    public class EntityKeyValuesService<TEntity> : IEntityKeyValuesService<TEntity>
        where TEntity : class
    {
        public string GetCacheKey(TEntity entity) => GetCacheKey(GetKeyValues(entity));

        public string GetCacheKey(IEnumerable<object> keyValues)
        {
            var sb = new StringBuilder(typeof(TEntity).FullName);
            foreach (var value in keyValues)
            {
                sb.Append($":{value}");
            }
            return sb.ToString();
        }

        public IEnumerable<object> GetKeyValues(TEntity entity)
        {
            var type = typeof(TEntity);
            var properties = type.GetProperties()
                .Where(prop => prop.IsDefined(typeof(KeyAttribute), false))
                .Select(prop => prop.Name);
            foreach (var property in properties)
            {
                yield return type.GetProperty(property).GetValue(entity);
            }
        }
    }
}
