using ECommerce.Api.Orders;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderProvider _provider;

        public OrdersController(IOrderProvider provider)
        {
            _provider = provider;
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetCustomerOrders(int customerId)
        {
            var result = await _provider.GetOrdersByCustomerIdAsync(customerId);
            if (!result.IsSuccess)
                return NotFound();

            return Ok(result.Orders);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var result = await _provider.GetOrdersAsync();
            if (!result.IsSuccess)
                return NotFound();

            return Ok(result.Orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var (IsSuccess, Order, _) = await _provider.GetOrderAsync(id);
            if (!IsSuccess)
                return NotFound();

            return Ok(Order);
        }
    }
}
