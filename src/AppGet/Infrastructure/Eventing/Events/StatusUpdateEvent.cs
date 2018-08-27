using System;

namespace AppGet.Infrastructure.Eventing.Events
{
    public abstract class StatusUpdateEvent : IEvent
    {
        public string Message { get; protected set; }

        public virtual void Print()
        {
            Console.WriteLine(Message);
        }
    }
}