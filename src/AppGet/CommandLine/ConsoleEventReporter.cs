using System.Threading.Tasks;
using AppGet.Infrastructure.Eventing;
using AppGet.ProgressTracker;

namespace AppGet.CommandLine
{
    public class ConsoleEventReporter : IHandle<ProgressUpdatedEvent>
    {
        private void HandleProgress(ProgressUpdatedEvent updatedEvent)
        {
        }


        public Task Handle(ProgressUpdatedEvent @event)
        {
            HandleProgress(@event);
            return Task.FromResult(0);
        }
    }
}