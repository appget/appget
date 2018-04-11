using AppGet.ProgressTracker;

namespace AppGet.FileTransfer
{
    public interface IFileTransferClient : IReportProgress
    {
        bool CanHandleProtocol(string source);
        void TransferFile(string source, string destinationFile);
        string ReadString(string source);
        string GetFileName(string source);
    }
}
