using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Services.Caching
{
    public static class CachePolicies 
    {
        public static CacheItemPolicy UserDataPolicy => new CacheItemPolicy()
        {
             SlidingExpiration = TimeSpan.FromSeconds(1),
        };

        public static CacheItemPolicy MessageDataPolicy => new CacheItemPolicy()
        {
            SlidingExpiration = TimeSpan.FromSeconds(2),
        };
    }
}
