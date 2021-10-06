using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IHttpClientFactory httpClientFactory, ILogger<ProductService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<(bool IsSuccess, IEnumerable<Product> Products, string ErrorMessage)> GetProductsAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ProductService");
                var response = await client.GetAsync($"api/products");

                if (!response.IsSuccessStatusCode)
                    return (false, null, response.ReasonPhrase);

                var content = await response.Content.ReadAsByteArrayAsync();
                var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                var result = JsonSerializer.Deserialize<IEnumerable<Product>>(content, options);

                return (true, result, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
