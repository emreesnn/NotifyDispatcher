
using NotifyDispatcher.Events;

namespace NotifyDispatcher.Dispatchers
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly NotificationDispatcher _notificationDispatcher;

        public EventDispatcher(NotificationDispatcher notificationDispatcher)
        {
            _notificationDispatcher = notificationDispatcher;
        }

        public async Task DispatchAsync<T>(T @event) where T : class
        {
           if(@event is PriceChangedEvent priceChanged) 
                await _notificationDispatcher.HandleAsync(priceChanged);
                
        }
    }
}
