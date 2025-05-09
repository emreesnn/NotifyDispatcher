using Microsoft.AspNetCore.Mvc;
using NotifyDispatcher.ProductWarcherService;

namespace NotifyDispatcher.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ProductWatcherService _productWatcherService;

        public TestController(ProductWatcherService productWarcherService)
        {
            _productWatcherService = productWarcherService;
        }


        [HttpGet]
        public async Task<IActionResult> GetProductFromAmazon(string url)
        {
            var product = await _productWatcherService.GetDataFromUrl(url);
            return Ok(product);
        }

    }
}
