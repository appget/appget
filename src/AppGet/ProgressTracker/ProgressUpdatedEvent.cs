
using AppGet.Infrastructure.Eventing;

namespace AppGet.ProgressTracker
{
    public class ProgressUpdatedEvent : IEvent
    {
        public long MaxValue { get; set; }
        public long Value { get; set; }

        public bool IsCompleted { get; set; }

        public decimal GetPercentCompleted()
        {
            if (MaxValue == 0 || Value == 0) return 0;
            return Value / (decimal)MaxValue * (decimal)100.0;
        }
    }
}