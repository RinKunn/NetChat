using System;
using System.Collections.Concurrent;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;
using NetChat.Services.Exceptions;

namespace NetChat.Services.Caching
{
    public class SafeDataCache : IDataCache
    {
        private readonly MemoryCache _cache = new MemoryCache(null);
        private readonly ConcurrentDictionary<string, SemaphoreSlim> _locks 
            = new ConcurrentDictionary<string, SemaphoreSlim>();

        public async Task<TItem> GetOrCreate<TItem>(string key, Func<Task<TItem>> createItem, CacheItemPolicy cacheItemPolicy)
        {
            if (string.IsNullOrEmpty(key))
                throw new CriticalException("Передан пустой ключ кэша",
                    new ArgumentNullException(nameof(key)));
            TItem cacheEntry;
            if(!TryGetValue(key, out cacheEntry))
            {
                SemaphoreSlim mylock = _locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));
                await mylock.WaitAsync();
                try
                {
                    if (!TryGetValue(key, out cacheEntry))
                    {
                        // Key not in cache, so get data.
                        cacheEntry = await createItem();
                        _cache.Set(key, cacheEntry, cacheItemPolicy);
                        CacheItemPolicy s = new CacheItemPolicy();
                    }
                }
                finally
                {
                    mylock.Release();
                }
            }
            return cacheEntry;
        }

        public bool TryGetValue<TItem>(string key, out TItem item)
        {
            if (!_cache.Contains(key))
            {
                item = default;
                return false;
            }
            else
            {
                item = (TItem)_cache.Get(key);
                return true;
            }
        }

    }
}
