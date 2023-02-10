using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Northwind.Model;
using Northwind.Web.Models;
using System.Collections;

namespace Northwind.Web.Tests
{
    [TestClass]
    public class AcceptanceTests
    {
        static IEnumerable<Category> Categories = new Category[]
        {
            new Category 
            { 
                CategoryId = 1, 
                CategoryName = "Beverages",
                Description = "Soft drinks, coffees, teas, beers, and ales",
                Products = new Product[]
                {
                    new Product { ProductName = "Chai" },
                    new Product { ProductName = "Chang" },
                    new Product { ProductName = "Guaraná Fantástica" },
                    new Product { ProductName = "Sasquatch Ale" },
                    new Product { ProductName = "Steeleye Stout" },
                    new Product { ProductName = "Côte de Blaye" },
                    new Product { ProductName = "Chartreuse verte" },
                    new Product { ProductName = "Ipoh Coffee" },
                    new Product { ProductName = "Laughing Lumberjack Lager" },
                    new Product { ProductName = "Outback Lager" },
                    new Product { ProductName = "Rhönbräu Klosterbier" },
                    new Product { ProductName = "Lakkalikööri" },

                }
            },
            new Category { CategoryId = 2, CategoryName = "Condiments", Description = "Sweet and savory sauces, relishes, spreads, and seasonings" },
            new Category { CategoryId = 3, CategoryName = "Confections", Description = "Desserts, candies, and sweet breads" },
            new Category { CategoryId = 4, CategoryName = "Dairy Products", Description = "Cheeses" },
            new Category { CategoryId = 5, CategoryName = "Grains/Cereals", Description = "Breads, crackers, pasta, and cereal" },
            new Category { CategoryId = 6, CategoryName = "Meat/Poultry", Description = "Prepared meats" },
            new Category { CategoryId = 7, CategoryName = "Produce", Description = "Dried fruit and bean curd" },
            new Category { CategoryId = 8, CategoryName = "Seafood", Description = "Seaweed and fish" },
        };

        [TestMethod]
        public async Task Home_ContainsAllLinks()
        {
            var client = GetHttpClient();

            var response = await client.GetStringAsync("/");
            var links = GetHomeLinks(response);

            links.Should().Contain(new string[] { "/", "/categories" });
        }

        [TestMethod]
        public async Task Index_ReturnViewResult_WithAllCategories()
        {
            var client = GetHttpClient();

            var response = await client.GetStringAsync("/categories");
            var result = CategoriesControllerIntegrationTesting.GetResultCategories(response);

            result.Should().BeEquivalentTo(Categories,
                options => options
                .Excluding(c => c.Picture)
                .Excluding(c => c.Products));
        }

        [TestMethod]
        public async Task Details_ReturnViewResult_WithCategoryDetails()
        {
            var client = GetHttpClient();

            var category = Categories.First();

            var response = await client.GetStringAsync($"/categories/details/{category.CategoryId}");
            var categoryViewModel = GetCategoryFromDetails(response);

            categoryViewModel.Should().BeEquivalentTo(category.ToViewModel(),
                options => options
                .Including(c => c.CategoryName)
                .Including(c => c.Description)
                .Including(c => c.ProductsCount)
                .Including(c => c.Products));
        }

        private static HttpClient GetHttpClient()
        {
            return new HttpClient() { BaseAddress = new Uri("http://localhost:5129/") };
        }

        private static NorthwindContext GetContext()
        {
            var options = new DbContextOptionsBuilder<NorthwindContext>()
                .UseSqlServer("Server=(local)\\sqlexpress;Database=Northwind;Trusted_Connection=True;MultipleActiveResultSets=true")
                .Options;

            return new NorthwindContext(options);
        }

        private static IEnumerable<string>? GetHomeLinks(string htmlSource)
        {
            var context = BrowsingContext.New(Configuration.Default);
            var document = context.OpenAsync(req => req.Content(htmlSource)).Result;

            return document.Links.Select(link => link.Attributes["href"]!.Value.ToLower());
        }

        public static CategoryViewModel GetCategoryFromDetails(string htmlSource)
        {
            var context = BrowsingContext.New(Configuration.Default);
            var document = context.OpenAsync(req => req.Content(htmlSource)).Result;

            var name = document.QuerySelector("dd[data-tid='category-name']")?
                .Text().Trim();
            var description = document.QuerySelector("dd[data-tid='category-description']")?
                .Text().Trim();
            var productsCount = Convert.ToInt32(document.QuerySelector("dd[data-tid='category-products-count']")?
                .Text().Trim());
            var products = document.QuerySelector("dd[data-tid='category-products']")?
                .Text().Trim();

            return new CategoryViewModel
            {
                CategoryName = name ?? "",
                Description = description,
                ProductsCount = productsCount,
                Products = products?
                .Replace("...", "").Split(", ")
                .Select(p => new Product { ProductName = p }) ?? Enumerable.Empty<Product>(),
            };
        }
    }
}
