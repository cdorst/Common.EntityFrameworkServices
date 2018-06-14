using System;
using System.Collections.Generic;
using System.Linq;
using static System.String;

namespace Common.EntityFrameworkServices.Factories
{
    public static class UniqueListIdentifierFactory<TRecord>
        where TRecord : class, IUniqueListRecord
    {
        public static string Create<TKey>(in List<TRecord> records, in Func<TRecord, TKey> keySelector)
            => Join(",", records.Select(keySelector));
    }
}
