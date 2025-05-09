namespace NotifyDispatcher.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string LastKnownPrice { get; set; }
        public string SiteName { get; set; }
        public string ProductUrl { get; set; }
        public ICollection<PriceHistory> PriceHistories { get; set; }
    }
}
