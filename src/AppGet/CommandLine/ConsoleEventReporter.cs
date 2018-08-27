using AppGet.Infrastructure.Eventing;
using AppGet.ProgressTracker;

namespace AppGet.CommandLine
{
    public class ConsoleEventReporter : IHandle<ProgressUpdatedEvent>
    {
        private void HandleProgress(ProgressUpdatedEvent updatedEvent)
        {
        }


        public void Handle(ProgressUpdatedEvent @event)
        {
            HandleProgress(@event);
        }
    }
}