using System;

namespace AppGet.ProgressTracker
{
    public interface IReportProgress
    {
        Action<ProgressState> OnStatusUpdates { get; set; }
        Action<ProgressState> OnCompleted { get; set; }
    }
}