using System.Collections.Generic;
using AppGet.Infrastructure.Composition;
using Autofac;

namespace AppGet.Infrastructure.Events
{
    public interface IEventHub
    {
        void Publish<T>(T message) where T : class, ITinyMessage;
    }

    public class EventHub : IEventHub
    {
        private readonly IEnumerable<IHandle> _handlers;

        public EventHub(IEnumerable<IHandle> handlers)
        {
            _handlers = handlers;
        }

        public void Publish<T>(T message) where T : class, ITinyMessage
        {
            foreach (var handler in _handlers)
            {
                if (handler is IHandle<T> h)
                {
                    h.Handle(message);
                }
            }
        }
    }


    public interface IHandle
    {

    }

    public interface IHandle<T> : IHandle where T : ITinyMessage
    {
        void Handle(T message);
    }
}
