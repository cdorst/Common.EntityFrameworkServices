using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.EntityFrameworkServices.Factories
{
    public static class UniqueListIdentifierFactory<TRecord>
        where TRecord : class, IUniqueListRecord
    {
        public static string Create<TKey>(List<TRecord> records, Func<TRecord, TKey> keySelector)
            => string.Join(",", records.Select(keySelector));
    }
}
