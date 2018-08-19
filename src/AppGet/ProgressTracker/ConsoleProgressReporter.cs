using System;
using AppGet.Infrastructure.Events;

namespace AppGet.ProgressTracker
{
    public class ConsoleProgressReporter : IStartupHandler
    {
        private readonly ITinyMessengerHub _hub;
        private static string _lastState = "";
        private const int PROGRESS_LENGTH = 20;


        public ConsoleProgressReporter(ITinyMessengerHub hub)
        {
            _hub = hub;
        }

        private string RenderBar(ProgressState state)
        {
            if (state.MaxValue.HasValue)
            {
                var percentCompleted = state.GetPercentCompleted();
                var percent = Math.Round(percentCompleted);
                var filled = (int)Math.Round(PROGRESS_LENGTH * percentCompleted / 100);

                var progress = new string('█', filled);

                return $"   {progress.PadRight(PROGRESS_LENGTH, '░')} {percent}%: {state.Value:N0} / {state.MaxValue:N0}";
            }

            if (state.Value != 0)
            {
                return $"   {state.Value:N0} downloaded";
            }

            return "";
        }

        private void HandleProgress(ProgressState state)
        {
            if (state.IsCompleted)
            {
                Console.WriteLine();
                Console.WriteLine();
                return;
            }

            var newState = RenderBar(state);

            if (_lastState == newState) return;

            _lastState = newState;
            Console.Write("\r{0}", newState);
        }

        public void OnApplicationStartup()
        {
            _hub.Subscribe<GenericTinyMessage<ProgressState>>(s => HandleProgress(s.Content));
        }
    }
}