using HtmlAgilityPack;
using NotifyDispatcher.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace NotifyDispatcher.ProductWarcherService
{
    public class ProductWatcherService
    {
        private readonly HttpClient _httpClient;

        public ProductWatcherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Product> GetDataFromUrl(string url)
        {
            var html = await _httpClient.GetStringAsync(url);

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var product = new Product();

            var priceWhole = doc.DocumentNode.SelectSingleNode("//span[@class='a-price-whole']")?.InnerText?.Trim();
            var priceFraction = doc.DocumentNode.SelectSingleNode("//span[@class='a-price-fraction']")?.InnerText?.Trim();
            var symbol = doc.DocumentNode.SelectSingleNode("//span[@class='a-price-symbol']")?.InnerText?.Trim();

            product.LastKnownPrice = $"{priceWhole},{priceFraction} {symbol}";

            return product;
        }
    }
}
