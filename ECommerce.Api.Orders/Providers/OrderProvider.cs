using AutoMapper;
using AutoMapper.QueryableExtensions;
using ECommerce.Api.Orders;
using ECommerce.Api.Orders.Db;
using ECommerce.Api.Orders.Db.Entities;
using ECommerce.Api.Orders.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Providers
{
    public class OrderProvider : IOrderProvider
    {
        private readonly OrderDbContext _dbContext;
        private readonly ILogger<OrderProvider> _logger;
        private readonly IMapper _mapper;

        public OrderProvider(
            OrderDbContext dbContext,
            ILogger<OrderProvider> logger,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;

            Seed();
        }

        public async Task<(bool, IEnumerable<OrderModel>, string)> GetOrdersByCustomerIdAsync(int customerId)
        {
            try
            {
                var customers = await _dbContext.Orders.Where(p => p.CustomerId == customerId)
                    .ProjectTo<OrderModel>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                if (customers?.Count < 0)
                    return (false, null, "Not found");

                return (true, customers, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }


        public async Task<(bool, OrderModel, string)> GetOrderAsync(int id)
        {
            try
            {
                var customer = await _dbContext.Orders.Where(p => p.Id == id)
                    .ProjectTo<OrderModel>(_mapper.ConfigurationProvider)
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

        public async Task<(bool, IEnumerable<OrderModel>, string)> GetOrdersAsync()
        {
            try
            {
                var customers = await _dbContext.Orders.ProjectTo<OrderModel>(_mapper.ConfigurationProvider).ToListAsync();

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
            if (!_dbContext.Orders.Any())
            {
                _dbContext.Orders.AddRange(new Order[]
                {
                    new Order{Id = 1, CustomerId = 1, OrderDate = DateTime.Now, Items = new List<OrderItem>
                    {
                        new OrderItem {Id = 1, OrderId = 1, ProductId = 1, Quantity = 2, UnitPrice = 20},
                        new OrderItem {Id = 2, OrderId = 1, ProductId = 2, Quantity = 4, UnitPrice = 20},
                        new OrderItem {Id = 3, OrderId = 1, ProductId = 3, Quantity = 2, UnitPrice = 200},
                        new OrderItem {Id = 4, OrderId = 1, ProductId = 4, Quantity = 2, UnitPrice = 100},
                    } },
                    new Order{Id = 2, CustomerId = 2, OrderDate = DateTime.Now, Items = new List<OrderItem>
                    {
                        new OrderItem {Id = 5, OrderId = 2, ProductId = 1, Quantity = 2, UnitPrice = 20},
                        new OrderItem {Id = 6, OrderId = 2, ProductId = 2, Quantity = 4, UnitPrice = 20},
                        new OrderItem {Id = 7, OrderId = 2, ProductId = 3, Quantity = 2, UnitPrice = 200},
                        new OrderItem {Id = 8, OrderId = 2, ProductId = 4, Quantity = 2, UnitPrice = 100},
                    } },
                    new Order{Id = 3, CustomerId = 3, OrderDate = DateTime.Now, Items = new List<OrderItem>
                    {
                        new OrderItem {Id = 9, OrderId = 3, ProductId = 1, Quantity = 2, UnitPrice = 20},
                        new OrderItem {Id = 10, OrderId = 3, ProductId = 2, Quantity = 4, UnitPrice = 20},
                        new OrderItem {Id = 11, OrderId = 3, ProductId = 3, Quantity = 2, UnitPrice = 200},
                        new OrderItem {Id = 12, OrderId = 3, ProductId = 4, Quantity = 2, UnitPrice = 100},
                    } },
                    new Order{Id = 4, CustomerId = 1, OrderDate = DateTime.Now, Items = new List<OrderItem>
                    {
                        new OrderItem {Id = 13, OrderId = 4, ProductId = 3, Quantity = 1, UnitPrice = 200},
                        new OrderItem {Id = 14, OrderId = 4, ProductId = 4, Quantity = 7, UnitPrice = 100},
                    } }
                });
                _dbContext.SaveChanges();
            }
        }
    }
}
