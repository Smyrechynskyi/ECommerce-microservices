using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerProvider _provider;

        public CustomersController(ICustomerProvider provider)
        {
            _provider = provider;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _provider.GetCustomersAsync();
            if (!result.IsSuccess)
                return NotFound();

            return Ok(result.Customers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var (IsSuccess, Customer, _) = await _provider.GetCustomerAsync(id);
            if (!IsSuccess)
                return NotFound();

            return Ok(Customer);
        }
    }
}
