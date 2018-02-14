
using System;

namespace AppGet.ProgressTracker
{
    public class ProgressState
    {
        const int PROGRESS_LENGTH = 20;

        public long Total { get; set; }
        public long Completed { get; set; }

        public decimal PercentCompleted => Completed / (decimal)Total * (decimal)100.0;

        public override string ToString()
        {
            var filled = (int)Math.Abs(PROGRESS_LENGTH * PercentCompleted / 100);

            var progress = new string('█', filled);

            return $"   [{progress.PadRight(PROGRESS_LENGTH, '░')}] {PercentCompleted:00}%: {Completed:N0} / {Total:N0}";
        }
    }
}