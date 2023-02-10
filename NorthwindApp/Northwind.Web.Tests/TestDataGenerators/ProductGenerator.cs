using Northwind.Model;
using Bogus;

namespace Northwind.Web.Tests.TestDataGenerators
{
    public class ProductGenerator : ITestDataGenerator<Product>
    {
        private readonly NorthwindContext? northwindContext;
        private Faker<Product> faker;

        public ProductGenerator(NorthwindContext? context = null)
        {
            northwindContext = context;

            faker = new Faker<Product>()
                .StrictMode(false)
                .RuleFor(p => p.ProductName, f => f.Commerce.ProductName())
                .RuleFor(p => p.UnitPrice, f => decimal.Parse(f.Commerce.Price()))
                .RuleFor(p => p.UnitsInStock, f => f.Random.Short(0))
                .RuleFor(p => p.UnitsOnOrder, f => f.Random.Short(0))
                .RuleFor(p => p.ReorderLevel, f => f.Random.Short(0))
                .RuleFor(p => p.Discontinued, f => f.Random.Bool());
        }

        public Product Generate()
        {
            var product = faker.Generate();
            northwindContext?.Products.Add(product);
            return product;
        }

        public IEnumerable<Product> Generate(int count)
        {
            var products = faker.Generate(count);
            northwindContext?.Products.AddRange(products);
            return products;
        }
    }
}
