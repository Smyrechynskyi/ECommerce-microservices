using ECommerce.Api.Search.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;

        public SearchService(
            IOrderService orderService,
            IProductService productService,
            ICustomerService customerService)
        {
            _orderService = orderService;
            _productService = productService;
            _customerService = customerService;
        }

        public async Task<(bool IsSuccess, dynamic SearchResults)> SearchAsync(int customerId)
        {
            var (IsSuccess, Orders, _) = await _orderService.GetOrdersAsync(customerId);
            var productsResult = await _productService.GetProductsAsync();
            var customerResult = await _customerService.GetCustomerAsync(customerId);

            if (productsResult.IsSuccess)
            {
                Orders.ToList()
                    .ForEach(o => o.Items.ToList()
                    .ForEach(oi => oi.ProductName = productsResult.Products.FirstOrDefault(p => p.Id == oi.ProductId)?.Name));
            }

            return (IsSuccess, IsSuccess ? new { Orders , customerResult.Customer } : null);
        }
    }
}
