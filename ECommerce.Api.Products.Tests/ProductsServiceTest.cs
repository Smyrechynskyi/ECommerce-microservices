using AutoMapper;
using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Db.Entities;
using ECommerce.Api.Products.Profiles;
using ECommerce.Api.Products.Providers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Api.Products.Tests
{
    public class ProductsServiceTest
    {
        [Fact]
        public async Task GetProductsReturnsAllProduct()
        {

            var options = new DbContextOptionsBuilder<ProductDbContext>()
                .UseInMemoryDatabase(nameof(GetProductsReturnsAllProduct))
                .Options;

            var dbContext = new ProductDbContext(options);
            CreateProducts(dbContext);

            var profile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            var mapper = new Mapper(configuration);

            var productProvider = new ProductsProvider(dbContext, null, mapper);
            var res = await  productProvider.GetProductsAsync();
            Assert.True(res.IsSuccess);
            Assert.True(res.Products.Any());
            Assert.Null(res.ErrorMessage);
        }

        [Fact]
        public async Task GetProductReturnsProductUsingValidId()
        {

            var options = new DbContextOptionsBuilder<ProductDbContext>()
                .UseInMemoryDatabase(nameof(GetProductReturnsProductUsingValidId))
                .Options;

            var dbContext = new ProductDbContext(options);
            CreateProducts(dbContext);

            var profile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            var mapper = new Mapper(configuration);

            var productProvider = new ProductsProvider(dbContext, null, mapper);
            var res = await productProvider.GetProductAsync(1);
            Assert.True(res.IsSuccess);
            Assert.NotNull(res.Product);
            Assert.True(res.Product.Id.Equals(1));
            Assert.Null(res.ErrorMessage);
        }

        [Fact]
        public async Task GetProductReturnsProductUsingInvalidId()
        {

            var options = new DbContextOptionsBuilder<ProductDbContext>()
                .UseInMemoryDatabase(nameof(GetProductReturnsProductUsingInvalidId))
                .Options;

            var dbContext = new ProductDbContext(options);
            CreateProducts(dbContext);

            var profile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            var mapper = new Mapper(configuration);

            var productProvider = new ProductsProvider(dbContext, null, mapper);
            var res = await productProvider.GetProductAsync(-1);
            Assert.False(res.IsSuccess);
            Assert.Null(res.Product);
            Assert.NotNull(res.ErrorMessage);
        }

        private void CreateProducts(ProductDbContext dbContext)
        {
            for (var i = 1; i == 10; i++)
            {
                dbContext.Products.Add(new Product
                {
                    Id = i,
                    Inventory = i * 10,
                    Name = $"Product {i}",
                    Price = i * 2
                });

                dbContext.SaveChanges();
            }
        }
    }
}
