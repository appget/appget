namespace AppGet.Infrastructure.Eventing
{
    public interface IHandle<in T> where T : IEvent
    {
        void Handle(T @event);
    }
}