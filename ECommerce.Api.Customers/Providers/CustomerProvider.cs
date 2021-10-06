using AutoMapper;
using AutoMapper.QueryableExtensions;
using ECommerce.Api.Customers.Db;
using ECommerce.Api.Customers.Db.Entities;
using ECommerce.Api.Customers.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Providers
{
    public class CustomerProvider : ICustomerProvider
    {
        private readonly CustomerDbContext _dbContext;
        private readonly ILogger<CustomerProvider> _logger;
        private readonly IMapper _mapper;

        public CustomerProvider(
            CustomerDbContext dbContext,
            ILogger<CustomerProvider> logger,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;

            Seed();
        }

        public async Task<(bool, CustomerModel, string)> GetCustomerAsync(int id)
        {
            try
            {
                var customer = await _dbContext.Customers.Where(p => p.Id == id)
                    .ProjectTo<CustomerModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();

                if (customer is null)
                    return (false, null, "Not found");

                return (true, customer, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<CustomerModel> Customers, string ErrorMessage)> GetCustomersAsync()
        {
            try
            {
                var customers = await _dbContext.Customers.ProjectTo<CustomerModel>(_mapper.ConfigurationProvider).ToListAsync();

                if (customers?.Count < 1)
                    return (false, null, "Not found");

                return (true, customers, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        private void Seed()
        {
            if (!_dbContext.Customers.Any())
            {
                _dbContext.Customers.AddRange(new Customer[]
                {
                    new Customer{Id = 1, Name = "Ivan", Email = "ivan@example.com"},
                    new Customer{Id = 2, Name = "Petro", Email = "petro@example.com"},
                    new Customer{Id = 3, Name = "Jon Larson", Email = "larson@example.com"},
                    new Customer{Id = 4, Name = "Kuzmich", Email = "kuzmich@gmail.com"},
                });
                _dbContext.SaveChanges();
            }
        }
    }
}
