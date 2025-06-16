using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Interfaces;

namespace Talabat.Repository
{
    public class BasketRepo : IBasketRepo
    {
        private readonly IDatabase _database;
        public BasketRepo( IConnectionMultiplexer Redis) 
        {
            _database = Redis.GetDatabase();
        }

        public async Task<bool> DeleteBacket(string id)
        {
            return await _database.KeyDeleteAsync(id);


        }

        public async Task<CustomerBasket?> GetBasket(string id)
        {
           var Backet =await _database.StringGetAsync(id);

            if (Backet.IsNull)  return null;
             return  JsonSerializer.Deserialize<CustomerBasket>(Backet); 
        } 

        public async Task<CustomerBasket?> UpdateBasket(CustomerBasket backet)
        {
            var jsonBacket = JsonSerializer.Serialize(backet);
            var Backet = await _database.StringSetAsync(backet.Id, jsonBacket, TimeSpan.FromDays(1));
             if (!Backet) return null;
             return await GetBasket(backet.Id);
           
        }
    }
}
