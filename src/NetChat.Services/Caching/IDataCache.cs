using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Services.Caching
{
    public interface IDataCache
    {
        Task<TItem> GetOrCreate<TItem>(string key, Func<Task<TItem>> createItem, CacheItemPolicy cacheItemPolicy);
    }
}
