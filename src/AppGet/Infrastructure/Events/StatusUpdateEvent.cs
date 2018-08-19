using System;

namespace AppGet.Infrastructure.Events
{
    public abstract class StatusUpdateEvent : ITinyMessage
    {
        public StatusUpdateEvent(object sender)
        {
            Sender = sender;
        }

        public object Sender { get; }
        public string Message { get; protected set; }

        public virtual void Print()
        {
            Console.WriteLine(Message);
        }
    }
}