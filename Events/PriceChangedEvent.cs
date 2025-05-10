namespace NotifyDispatcher.Events
{
    public class PriceChangedEvent
    {
        public string ProductTitle { get; set; }
        public decimal OldPrice { get; set; }
        public decimal NewPrice { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}
