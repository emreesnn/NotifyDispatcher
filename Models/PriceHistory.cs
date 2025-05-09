namespace NotifyDispatcher.Models
{
    public class PriceHistory
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Price { get; set; }
        public DateTime CheckedAt { get; set; }
        public Product Product { get; set; }
    }
}
