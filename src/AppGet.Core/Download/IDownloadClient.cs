using AppGet.Core.ProgressTracker;

namespace AppGet.Core.Download
{
    public interface IDownloadClient : IReportProgress
    {
        bool CanHandleProtocol(string url);
        void DownloadFile(string url, string fileName);
    }
}
