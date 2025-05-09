using HtmlAgilityPack;
using NotifyDispatcher.Data;
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

            var productTitle = doc.DocumentNode.SelectSingleNode("//span[@id='productTitle']")?.InnerText?.Trim();
            var priceWhole = doc.DocumentNode.SelectSingleNode("//span[@class='a-price-whole']")?.InnerText?.Trim();
            var priceFraction = doc.DocumentNode.SelectSingleNode("//span[@class='a-price-fraction']")?.InnerText?.Trim();
            var symbol = doc.DocumentNode.SelectSingleNode("//span[@class='a-price-symbol']")?.InnerText?.Trim();

            var product = new Product
            {
                Title = productTitle,
                LastKnownPrice = $"{priceWhole},{priceFraction} {symbol}",
                SiteName = "Amazon",
                ProductUrl = url
            };
               
            

            return product;
        }
    }
}
