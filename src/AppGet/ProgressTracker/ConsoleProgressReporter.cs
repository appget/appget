using System;

namespace AppGet.ProgressTracker
{
    public static class ConsoleProgressReporter
    {
        private static string _lastState = "";

        public static void HandleProgress(ProgressState state)
        {
            var newState = state.ToString();
            if (_lastState == newState) return;

            _lastState = newState;
            Console.Write("\r{0}", state);
        }


        public static void HandleCompleted(ProgressState obj)
        {
            HandleProgress(obj);
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}