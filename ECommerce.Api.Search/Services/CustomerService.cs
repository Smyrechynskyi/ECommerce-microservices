using ECommerce.Api.Search.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<OrderService> _logger;

        public CustomerService(
            IHttpClientFactory httpClientFactory,
            ILogger<OrderService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<(bool IsSuccess, dynamic Customer, string ErrorMessage)> GetCustomerAsync(int customerId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("CustomerService");
                var response = await client.GetAsync($"api/customers/{customerId}");

                if (!response.IsSuccessStatusCode)
                    return (false, null, response.ReasonPhrase);

                var content = await response.Content.ReadAsByteArrayAsync();
                var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                var result = JsonSerializer.Deserialize<dynamic>(content, options);

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
