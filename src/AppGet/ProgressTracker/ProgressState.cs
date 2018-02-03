
namespace AppGet.ProgressTracker
{
    public class ProgressState
    {
        public long Total { get; set; }
        public long Completed { get; set; }

        public decimal PercentCompleted => Completed / (decimal)Total * (decimal)100.0;

        public override string ToString()
        {
            return $"{PercentCompleted:00}%: {Completed:N0} / {Total:N0}";
        }
    }
}