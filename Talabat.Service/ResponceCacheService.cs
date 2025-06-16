using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.ServiceInterfaces;

namespace Talabat.Service
{
    public class ResponceCacheService : IResponseCacheService
    {
        private readonly IDatabase _database;
        public ResponceCacheService(IConnectionMultiplexer Redis)
        {
            _database = Redis.GetDatabase();



        }
        public async Task CacheResponceAsync(string key, object value, TimeSpan ExpireTime)
        {
            if (value is null) return;
            var Options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var jsonValue = JsonSerializer.Serialize(value, Options); // convert to String(json)
           await _database.StringSetAsync(key, jsonValue, ExpireTime);

           
        }

        public async Task<string?> GetCacheDataAsync(string key)
        {
            var CacheResponce = await _database.StringGetAsync(key);

            if (CacheResponce.IsNull) return null;
            return CacheResponce;
        }
    }
}
