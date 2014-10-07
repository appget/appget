namespace AppGet.ProgressTracker
{
    public class ProgressState
    {
        public long Total { get; set; }
        public long Completed { get; set; }

        public decimal PercentCompleted
        {
            get
            {
                return Total / Completed * 100;
            }
        }
    }
}