using System;

namespace AppGet.ProgressTracker
{
    public interface IReportProgress
    {
        Action<ProgressState> OnStatusUpdated { get; set; }
    }
}