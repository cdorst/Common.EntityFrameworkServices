﻿using System.Threading.Tasks;

namespace Common.EntityFrameworkServices
{
    public interface ICacheWithDefaultKeysService<TRecord>
        where TRecord : class
    {
        Task<TRecord> FindAsync(params object[] keyValues);
        Task RemoveAsync(params object[] keyValues);
        Task SaveAsync(TRecord record);
    }
}
