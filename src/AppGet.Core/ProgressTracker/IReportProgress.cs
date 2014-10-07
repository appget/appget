using System;

namespace AppGet.Core.ProgressTracker
{
    public interface IReportProgress
    {
        Action<ProgressState> OnStatusUpdates { get; set; }
    }
}