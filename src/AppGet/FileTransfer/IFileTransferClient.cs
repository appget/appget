using System;
using System.Threading.Tasks;
using AppGet.ProgressTracker;

namespace AppGet.FileTransfer
{
    public interface IFileTransferClient
    {
        bool CanHandleProtocol(string source);
        Task TransferFile(string source, string destinationFile, Action<ProgressUpdatedEvent> progressCallback);
        Task<string> ReadString(string source);
        Task<string> GetFileName(string source);
    }
}