using System.Threading.Tasks;
using AppGet.Manifests;
using AppGet.ProgressTracker;

namespace AppGet.FileTransfer
{
    public interface IFileTransferClient : IReportProgress
    {
        bool CanHandleProtocol(string source);
        void TransferFile(string source, string destinationFile, FileVerificationInfo fileVerificationInfo = null);
        Task<string> ReadString(string source);
        Task<string> GetFileName(string source);
    }
}
