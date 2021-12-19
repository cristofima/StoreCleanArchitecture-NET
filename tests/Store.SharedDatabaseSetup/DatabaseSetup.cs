using Bogus;
using Store.ApplicationCore.Entities;
using Store.ApplicationCore.Utils;
using Store.Infrastructure.Persistence.Contexts;

namespace Store.SharedDatabaseSetup
{
    public static class DatabaseSetup
    {
        public static void SeedData(StoreContext context)
        {
            context.Products.RemoveRange(context.Products);

            var productIds = 1;
            var fakeProducts = new Faker<Product>()
                .RuleFor(o => o.Name, f => $"Product {productIds}")
                .RuleFor(o => o.Description, f => $"Description {productIds}")
                .RuleFor(o => o.Id, f => productIds++)
                .RuleFor(o => o.Stock, f => f.Random.Number(1, 50))
                .RuleFor(o => o.Price, f => f.Random.Double(0.01, 100))
                .RuleFor(o => o.CreatedAt, f => DateUtil.GetCurrentDate())
                .RuleFor(o => o.UpdatedAt, f => DateUtil.GetCurrentDate());

            var products = fakeProducts.Generate(10);

            context.AddRange(products);

            context.SaveChanges();
        }
    }
}