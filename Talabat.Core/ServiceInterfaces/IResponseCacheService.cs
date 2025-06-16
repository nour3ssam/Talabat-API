using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.ServiceInterfaces
{
    public interface IResponseCacheService
    {
        // Set Cache Data
        Task CacheResponceAsync (string key, object value , TimeSpan ExpireTime);
        // Get Cache Data
        Task<string?> GetCacheDataAsync (string key);

    }
} 
