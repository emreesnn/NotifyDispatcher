namespace NotifyDispatcher.Dispatchers
{
    public interface IEventDispatcher
    {
        Task DispatchAsync<T>(T @event) where T : class;
    }
}
