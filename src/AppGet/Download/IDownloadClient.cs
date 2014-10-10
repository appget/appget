using AppGet.ProgressTracker;

namespace AppGet.Download
{
    public interface IDownloadClient : IReportProgress
    {
        bool CanHandleProtocol(string url);
        void DownloadFile(string url, string destination);
    }
}
