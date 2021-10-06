using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.Api.Products.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsProvider _provider;

        public ProductsController(IProductsProvider provider)
        {
            _provider = provider;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _provider.GetProductsAsync();
            if (!result.IsSuccess)
                return NotFound();

            return Ok(result.Products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var result = await _provider.GetProductAsync(id);
            if (!result.IsSuccess)
                return NotFound();

            return Ok(result.Product);
        }
    }
}
