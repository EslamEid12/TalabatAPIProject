using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreDBContext dBContext)
        {

            if (!dBContext.ProductBrand.Any())
            {
                var brandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                if (brands is not null && brands.Count() > 0)
                {
                    foreach (var brand in brands)
                        await dBContext.Set<ProductBrand>().AddAsync(brand);
                    await dBContext.SaveChangesAsync();
                }
            }


            if (!dBContext.ProductTypes.Any())
            {
                var TypesData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(TypesData);
                if (types is not null && types.Count > 0)
                {
                    foreach (var type in types)
                        await dBContext.Set<ProductType>().AddAsync(type);

                    await dBContext.SaveChangesAsync();
                }
            }

            if (!dBContext.Products.Any())
            {

                var productsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                foreach (var product in products)
                    dBContext.Set<Product>().Add(product);

                await dBContext.SaveChangesAsync();
            }


            if (!dBContext.DeliveryMethods.Any())
            {

                var deleveryMethodData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");
                var deleveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deleveryMethodData);

                foreach (var deleveryMethod in deleveryMethods)
                    dBContext.Set<DeliveryMethod>().Add(deleveryMethod);

                await dBContext.SaveChangesAsync();
            }
        }
    }
}
