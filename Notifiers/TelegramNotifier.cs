using NotifyDispatcher.Events;

namespace NotifyDispatcher.Notifiers
{
    public class TelegramNotifier
    {
        private readonly string _botToken;
        private readonly string _chatId;

        public TelegramNotifier(IConfiguration configuration)
        {
            _botToken = configuration["Telegram:BotToken"];
            _chatId = configuration["Telegram:ChatId"];
        }

        public async Task SendNotificationASync(PriceChangedEvent priceChangedEvent)
        {
            using var client = new HttpClient();
            var message = $"Fiyat düştü!\nÜrün ID: {priceChangedEvent.ProductId}\n" +
                          $"Eski fiyat: {priceChangedEvent.OldPrice}\nYeni fiyat: {priceChangedEvent.NewPrice}";

            var url = $"https://api.telegram.org/bot{_botToken}/sendMessage?chat_id={_chatId}&text={message}";

            await client.GetAsync(url);
        }
    }
}
