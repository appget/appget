
using System;

namespace AppGet.ProgressTracker
{
    public class ProgressState
    {
        const int PROGRESS_LENGTH = 20;

        public long? Total { get; set; }
        public long Completed { get; set; }

        public decimal PercentCompleted => Completed / ((decimal?)Total ?? 0) * (decimal)100.0;

        public override string ToString()
        {
            if (Total.HasValue)
            {
                var filled = (int)Math.Round(PROGRESS_LENGTH * PercentCompleted / 100);

                var progress = new string('█', filled);

                return $"   {progress.PadRight(PROGRESS_LENGTH, '░')} {Math.Round(PercentCompleted)}%: {Completed:N0} / {Total:N0}";
            }
            else
            {
                return $"   {Completed:N0} downloaded";
            }
        }
    }
}