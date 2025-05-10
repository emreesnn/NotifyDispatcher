using HtmlAgilityPack;
using NotifyDispatcher.Data;
using NotifyDispatcher.Dispatchers;
using NotifyDispatcher.Events;
using NotifyDispatcher.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace NotifyDispatcher.ProductWarcherService
{
    public class ProductWatcherService
    {
        private readonly HttpClient _httpClient;
        private readonly ProductRepository _productRepository;
        private readonly IEventDispatcher _eventDispatcher;

        public ProductWatcherService(HttpClient httpClient, 
            ProductRepository productRepository,
            IEventDispatcher eventDispatcher)
        {
            _httpClient = httpClient;
            _productRepository = productRepository;
            _eventDispatcher = eventDispatcher;
        }

        public async Task<Product> GetDataFromUrl(string url)
        {
            var html = await _httpClient.GetStringAsync(url);

            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            
            //Reading New Price
            var priceWhole = doc.DocumentNode.SelectSingleNode("//span[@class='a-price-whole']")?.InnerText?.Trim();
            var priceFraction = doc.DocumentNode.SelectSingleNode("//span[@class='a-price-fraction']")?.InnerText?.Trim();

            var newProduct = new Product
            {
                LastKnownPrice = $"{priceWhole}{priceFraction}",
            };

            //Is there any same product in db check
            Product product = await _productRepository.GetByUrl(url);
            if(product == null) {
                var productTitle = doc.DocumentNode.SelectSingleNode("//span[@id='productTitle']")?.InnerText?.Trim();
                newProduct.Title = productTitle;
                newProduct.SiteName = "Amazon";
                newProduct.ProductUrl = url;

                await _productRepository.CreateProductAsync(newProduct);
                product = newProduct;
            }

            decimal LastKnownPrice = decimal.Parse(product.LastKnownPrice);
            decimal newPrice = decimal.Parse(newProduct.LastKnownPrice);
            
            if(newPrice != LastKnownPrice)
            {
                //TODO: priceHistory Oluştur
                await _productRepository.UpdateProductPriceAsync(product.Id, newProduct.LastKnownPrice);
                PriceChangedEvent priceChangedEvent = new PriceChangedEvent
                {
                    ProductTitle = product.Title,
                    OldPrice = LastKnownPrice,
                    NewPrice = newPrice,
                    ChangedAt = DateTime.Now
                };

                await _eventDispatcher.DispatchAsync(priceChangedEvent);

            }

            return product;
        }
    }
}
