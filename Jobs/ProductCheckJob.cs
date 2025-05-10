using NotifyDispatcher.Data;
using NotifyDispatcher.ProductWarcherService;

namespace NotifyDispatcher.Jobs
{
    public class ProductCheckJob
    {
        private readonly ProductWatcherService _productWatcherService;
        private readonly ProductRepository _productRepository;

        public ProductCheckJob(ProductWatcherService productWarcherService, ProductRepository productRepository)
        {
            _productWatcherService = productWarcherService;
            _productRepository = productRepository;
        }

        public async Task RunAsync()
        {
            var products = await _productRepository.GetAll();

            foreach (var product in products)
            {
                await _productWatcherService.GetDataFromUrl(product.ProductUrl);    
            }
 
        }
    }
}
