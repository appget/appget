using AppGet.ProgressTracker;

namespace AppGet.FileTransfer
{
    public interface IFileTransferClient : IReportProgress
    {
        bool CanHandleProtocol(string source);
        string TransferFile(string source, string destinationDirectory);
        string ReadString(string source);
        string GetFileName(string source);
    }
}
