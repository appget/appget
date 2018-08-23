using AppGet.ProgressTracker;

namespace AppGet.Infrastructure.Events
{
    public class ProgressChangeEvent : ITinyMessage
    {
        public ProgressState Progress { get; }

        public ProgressChangeEvent(object sender, ProgressState progress)
        {
            Progress = progress;
            Sender = sender;
        }

        public object Sender { get; }
    }
}