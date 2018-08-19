
namespace AppGet.ProgressTracker
{
    public class ProgressState
    {
        public long? MaxValue { get; set; }
        public long Value { get; set; }

        public bool IsCompleted { get; set; }

        public decimal GetPercentCompleted()
        {
            if (!MaxValue.HasValue || MaxValue.Value == 0) return 0;
            return Value / ((decimal?)MaxValue ?? 0) * (decimal)100.0;
        }
    }
}