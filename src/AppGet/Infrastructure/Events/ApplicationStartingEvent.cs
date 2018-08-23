namespace AppGet.Infrastructure.Events
{
    public class ApplicationStartingEvent : ITinyMessage
    {
        public ApplicationStartingEvent(object sender)
        {
            Sender = sender;
        }

        public object Sender { get; }
    }
}