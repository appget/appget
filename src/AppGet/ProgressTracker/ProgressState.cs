using System;

namespace AppGet.ProgressTracker
{
    public class ProgressState
    {
        private const int PROGRESS_LENGTH = 20;

        public long? Total { get; set; }
        public long Completed { get; set; }

        private decimal GetPercentCompleted()
        {
            if (!Total.HasValue || Total.Value == 0) return 0;
            return Completed / ((decimal?)Total ?? 0) * (decimal)100.0;
        }

        public override string ToString()
        {

            if (Total.HasValue)
            {
                var percentCompleted = GetPercentCompleted();
                var percent = Math.Round(percentCompleted);
                var filled = (int)Math.Round(PROGRESS_LENGTH * percentCompleted / 100);

                var progress = new string('█', filled);

                return $"   {progress.PadRight(PROGRESS_LENGTH, '░')} {percent}%: {Completed:N0} / {Total:N0}";
            }

            if (Completed != 0)
            {
                return $"   {Completed:N0} downloaded";
            }

            return "";
        }
    }
}