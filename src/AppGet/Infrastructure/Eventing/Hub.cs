using System;
using System.Collections.Generic;
using System.Linq;
using DryIoc;

namespace AppGet.Infrastructure.Eventing
{
    public interface IHub
    {
        void Publish<TEvent>(TEvent @event) where TEvent : IEvent;
        void Subscribe<TEvent>(object owner, Action<TEvent> callback);
        void UnSubscribe(object owner);
    }

    public class Hub : IHub
    {
        private class EventCallBack
        {
            public EventCallBack(object owner, object callback)
            {
                Callback = callback;
                Owner = owner;
            }

            public object Callback { get; }
            public object Owner { get; }
        }

        private readonly IContainer _container;

        private static readonly Dictionary<Type, List<EventCallBack>> CallbackRegistration = new Dictionary<Type, List<EventCallBack>>();

        public Hub(IContainer container)
        {
            _container = container;
        }

        public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            var handlers = _container.ResolveMany<IHandle<TEvent>>();

            foreach (var handler in handlers)
            {
                handler.Handle(@event);
            }

            var callBacks = GetCallBacks<TEvent>().Select(c => c.Callback).Cast<Action<TEvent>>().ToList();

            foreach (var callback in callBacks)
            {
                callback(@event);
            }
        }

        private static List<EventCallBack> GetCallBacks<TEvent>()
        {
            CallbackRegistration.TryGetValue(typeof(TEvent), out var handlers);
            if (handlers == null)
            {
                handlers = new List<EventCallBack>();
                CallbackRegistration.Add(typeof(TEvent), handlers);
            }

            return handlers;
        }

        public void Subscribe<TEvent>(object owner, Action<TEvent> callback)
        {
            var handlers = GetCallBacks<TEvent>();
            if (!handlers.Any(h => h.Owner == owner && ReferenceEquals(h.Callback, callback)))
            {
                handlers.Add(new EventCallBack(owner, callback));
            }
        }


        public void UnSubscribe(object owner)
        {
            foreach (var eventCallbacks in CallbackRegistration)
            {
                eventCallbacks.Value.RemoveAll(c => c.Owner == owner);
            }
        }
    }
}
