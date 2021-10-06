using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpPost]
        public async Task<IActionResult> SearchAsync(SearchTerm searchTerm)
        {
            var result = await _searchService.SearchAsync(searchTerm.CustomerId);

            if (!result.IsSuccess)
                return NotFound();

            return Ok(result.SearchResults);
        }
    }
}
