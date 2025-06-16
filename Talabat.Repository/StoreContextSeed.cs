using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public static class StoreContextSeed
    {
        public static async Task Seedasync(StoreContext dbContext)
        {
            if (!dbContext.Set<ProductBrand>().Any())
            {
                var GetBrand = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
                var brand = JsonSerializer.Deserialize<List<ProductBrand>>(GetBrand);
                if (brand?.Count>0)
                {
                    foreach (var item in brand)
                    {
                        await dbContext.Set<ProductBrand>().AddAsync(item);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }



            if (!dbContext.Set<ProductType>().Any())
            {
                var GetType = File.ReadAllText("../Talabat.Repository/Data/DataSeed/types.json");
                var Type = JsonSerializer.Deserialize<List<ProductType>>(GetType);
                if (Type?.Count>0)
                {
                    foreach (var item in Type)
                    {
                        await dbContext.Set<ProductType>().AddAsync(item);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }



            if (!dbContext.Set<Product>().Any())
            {
            var GetProduct = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
                var Product = JsonSerializer.Deserialize<List<Product>>(GetProduct);
                if (Product?.Count > 0)
                {
                    foreach (var item in Product)
                    {
                        await dbContext.Set<Product>().AddAsync(item);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }


            if (!dbContext.DeliveryMethod.Any())
            {
                var DeliveryMethodData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");
                var Data = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodData);
                if (Data?.Count > 0)
                {
                    foreach (var item in Data)
                    {
                        await dbContext.DeliveryMethod.AddAsync(item);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

        }

    }
}
