using System.Threading.Tasks;
using AppGet.ProgressTracker;

namespace AppGet.FileTransfer
{
    public interface IFileTransferClient : IReportProgress
    {
        bool CanHandleProtocol(string source);
        void TransferFile(string source, string destinationFile);
        Task<string> ReadString(string source);
        Task<string> GetFileName(string source);
    }
}