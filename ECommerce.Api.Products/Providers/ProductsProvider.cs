using AutoMapper;
using AutoMapper.QueryableExtensions;
using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Db.Entities;
using ECommerce.Api.Products.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Products.Providers
{
    public class ProductsProvider : IProductsProvider
    {
        private readonly ProductDbContext _dbContext;
        private readonly ILogger<ProductsProvider> _logger;
        private readonly IMapper _mapper;

        public ProductsProvider(
            ProductDbContext dbContext,
            ILogger<ProductsProvider> logger,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;

            Seed();
        }

        public async Task<(bool IsSuccess, ProductModel Product, string ErrorMessage)> GetProductAsync(int id)
        {
            try
            {
                var product = await _dbContext.Products.Where(p => p.Id == id)
                    .ProjectTo<ProductModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();

                if (product is null)
                    return (false, null, "Not found");

                return (true, product, null);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<ProductModel> Products, string ErrorMessage)> GetProductsAsync()
        {
            try
            {
                var products = await _dbContext.Products.ProjectTo<ProductModel>(_mapper.ConfigurationProvider).ToListAsync();

                if (products?.Count < 1)
                    return (false, null, "Not found");

                return (true, products, null);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        private void Seed()
        {
            if (!_dbContext.Products.Any())
            {
                _dbContext.Products.AddRange(new Product[]
                {
                    new Product{Id = 1, Name = "keyboard", Price = 20, Inventory = 100},
                    new Product{Id = 2, Name = "Mouse", Price = 20, Inventory = 100},
                    new Product{Id = 3, Name = "Screen 19", Price = 200, Inventory = 100},
                    new Product{Id = 4, Name = "Screen 22", Price = 100, Inventory = 100},
                    new Product{Id = 5, Name = "Tablet A", Price = 300, Inventory = 100},
                    new Product{Id = 6, Name = "Tablet B", Price = 320, Inventory = 100},
                    new Product{Id = 7, Name = "Tablet C", Price = 320, Inventory = 0},
                });
                _dbContext.SaveChanges();
            }
        }
    }
}
