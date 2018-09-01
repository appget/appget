using System.Threading.Tasks;

namespace AppGet.Infrastructure.Eventing
{
    public interface IHandle<in T> where T : IEvent
    {
        Task Handle(T @event);
    }
}