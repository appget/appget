using System;
using AppGet.Infrastructure.Events;
using AppGet.ProgressTracker;

namespace AppGet.CommandLine
{
    public class ConsoleEventReporter : IStartupHandler
    {
        private readonly ITinyMessengerHub _hub;


        public ConsoleEventReporter(ITinyMessengerHub hub)
        {
            _hub = hub;
        }

        private void HandleProgress(ProgressState state)
        {
        }

        public void OnApplicationStartup()
        {
            _hub.Subscribe<GenericTinyMessage<ProgressState>>(s => HandleProgress(s.Content));
        }
    }
}