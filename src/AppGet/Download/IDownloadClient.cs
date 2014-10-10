using AppGet.ProgressTracker;

namespace AppGet.Download
{
    public interface IDownloadClient : IReportProgress
    {
        bool CanHandleProtocol(string source);
        void DownloadFile(string source, string destination);
        string ReadString(string source);
    }
}
