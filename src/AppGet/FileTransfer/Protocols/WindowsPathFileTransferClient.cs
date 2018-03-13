using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AppGet.Manifests;
using AppGet.ProgressTracker;

namespace AppGet.FileTransfer.Protocols
{
    public class WindowsPathFileTransferClient : IFileTransferClient
    {
        private static readonly Regex WindowsPathRegex = new Regex(@"^\\\\|^[a-z]:\\", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public bool CanHandleProtocol(string source)
        {
            return WindowsPathRegex.IsMatch(source);
        }

        public void TransferFile(string source, string destinationFile, FileVerificationInfo fileVerificationInfo)
        {
            var progress = new ProgressState
            {
                Total = 1
            };

            //TODO: move this to a Copy using streams/that way we can provide progress
            File.Copy(source, destinationFile);

            progress.Completed = 1;

            OnStatusUpdated?.Invoke(progress);

            OnCompleted?.Invoke(progress);
        }

        public async Task<string> ReadString(string source)
        {
            using (var reader = File.OpenText(source))
            {
                return await reader.ReadToEndAsync();
            }
        }

        public Task<string> GetFileName(string source)
        {
            return Task.FromResult(new FileInfo(source).Name);
        }

        public Action<ProgressState> OnStatusUpdated { get; set; }
        public Action<ProgressState> OnCompleted { get; set; }
    }
}
