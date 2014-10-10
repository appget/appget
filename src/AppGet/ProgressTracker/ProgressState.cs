
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
                return Completed / (decimal)Total * (decimal)100.0;
            }
        }

        public override string ToString()
        {
            return string.Format("{0:00}%: {1:N0} / {2:N0}", PercentCompleted, Completed, Total);
        }
    }
}