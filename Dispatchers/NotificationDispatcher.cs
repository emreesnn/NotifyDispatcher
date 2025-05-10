using NotifyDispatcher.Events;
using NotifyDispatcher.Notifiers;

namespace NotifyDispatcher.Dispatchers
{
    public class NotificationDispatcher
    {
        private readonly TelegramNotifier _telegramNotifier;

        public NotificationDispatcher(TelegramNotifier telegramNotifier)
        {
            _telegramNotifier = telegramNotifier;
        }

        public async Task HandleAsync(PriceChangedEvent priceChangedEvent)
        {
            await _telegramNotifier.SendNotificationASync(priceChangedEvent);
        }
    }
}
