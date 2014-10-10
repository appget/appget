using System;

namespace AppGet.ProgressTracker
{
    public static class ConsoleProgressReporter
    {
        public static void HandleProgress(ProgressState state)
        {
            Console.Write("\r{0}%   ", state);
        }

        public static void HandleCompleted(ProgressState obj)
        {
            Console.WriteLine();
        }
    }
}