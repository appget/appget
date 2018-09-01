using System;
using System.Threading.Tasks;
using AppGet.Extensions;
using AppGet.Infrastructure.Eventing;
using AppGet.ProgressTracker;

namespace AppGet.CommandLine
{
    public class ConsoleProgressReporter : IHandle<ProgressUpdatedEvent>
    {
        private static string _lastState = "";
        private const int PROGRESS_LENGTH = 20;


        private string RenderBar(ProgressUpdatedEvent updatedEvent)
        {
            if (updatedEvent.MaxValue != 0)
            {
                var percentCompleted = updatedEvent.GetPercentCompleted();
                var percent = Math.Round(percentCompleted);
                var filled = (int)Math.Round(PROGRESS_LENGTH * percentCompleted / 100);

                var progress = new string('█', filled);

                return $"   {progress.PadRight(PROGRESS_LENGTH, '░')} {percent}%: {updatedEvent.Value.ToFileSize()} / {updatedEvent.MaxValue.ToFileSize()}";
            }

            if (updatedEvent.Value != 0)
            {
                return $"   {updatedEvent.Value:N0} downloaded";
            }

            return "";
        }

        private void HandleProgress(ProgressUpdatedEvent updatedEvent)
        {
            if (updatedEvent.IsCompleted)
            {
                Console.WriteLine();
                Console.WriteLine();
                return;
            }

            var newState = RenderBar(updatedEvent);

            if (_lastState == newState) return;

            _lastState = newState;
            Console.Write("\r{0}", newState);
        }

        public Task Handle(ProgressUpdatedEvent @event)
        {
            HandleProgress(@event);
            return Task.FromResult(0);
        }
    }
}