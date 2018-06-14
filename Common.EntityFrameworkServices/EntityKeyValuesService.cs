using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using static System.String;

namespace Common.EntityFrameworkServices
{
    public class EntityKeyValuesService<TEntity> : IEntityKeyValuesService<TEntity>
        where TEntity : class
    {
        private static readonly Func<PropertyInfo, string> _propertiesSelect = prop => prop.Name;
        private static readonly Func<PropertyInfo, bool> _propertiesWhere = prop => prop.IsDefined(typeof(KeyAttribute), false);

        public string GetCacheKey(in TEntity entity) => GetCacheKey(GetKeyValues(entity));

        public string GetCacheKey(in IEnumerable<object> keyValues)
        {
            var sb = new StringBuilder(typeof(TEntity).FullName);
            foreach (var value in keyValues) sb.Append(Concat(":", value));
            return sb.ToString();
        }

        public IEnumerable<object> GetKeyValues(TEntity entity)
        {
            var type = typeof(TEntity);
            var properties = type.GetProperties()
                .Where(_propertiesWhere)
                .Select(_propertiesSelect);
            foreach (var property in properties)
            {
                yield return type.GetProperty(property).GetValue(entity);
            }
        }
    }
}
