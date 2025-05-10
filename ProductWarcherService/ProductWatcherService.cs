using HtmlAgilityPack;
using NotifyDispatcher.Data;
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

        public ProductWatcherService(HttpClient httpClient, ProductRepository productRepository)
        {
            _httpClient = httpClient;
            _productRepository = productRepository;
        }

        public async Task<Product> GetDataFromUrl(string url)
        {
            var html = await _httpClient.GetStringAsync(url);

            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            

            //Reading New Price
            var priceWhole = doc.DocumentNode.SelectSingleNode("//span[@class='a-price-whole']")?.InnerText?.Trim();
            var priceFraction = doc.DocumentNode.SelectSingleNode("//span[@class='a-price-fraction']")?.InnerText?.Trim();
            var symbol = doc.DocumentNode.SelectSingleNode("//span[@class='a-price-symbol']")?.InnerText?.Trim();
            var newProduct = new Product
            {
                LastKnownPrice = $"{priceWhole},{priceFraction} {symbol}",
            };
            //Veritabanında bu url varmıydı kontrolü
            Product product = await _productRepository.GetByUrl(url);
            if(product == null) {
                var productTitle = doc.DocumentNode.SelectSingleNode("//span[@id='productTitle']")?.InnerText?.Trim();
                newProduct.Title = productTitle;
                newProduct.SiteName = "Amazon";
                newProduct.ProductUrl = url;

                await _productRepository.CreateProductASync(newProduct);
            }

            decimal LastKnownPrice = decimal.Parse(product.LastKnownPrice);
            decimal newPrice = decimal.Parse(newProduct.LastKnownPrice);
            
            if(newPrice != LastKnownPrice)
            {
                //TODO: priceHistory Oluştur
                await _productRepository.UpdateProductPriceAsync(product.Id, newProduct.LastKnownPrice);
                PriceChangedEvent priceChangedEvent = new PriceChangedEvent
                {
                    ProductId = product.Id,
                    OldPrice = LastKnownPrice,
                    NewPrice = newPrice,
                    ChangedAt = DateTime.Now
                };
            }

            return product;
        }
    }
}
