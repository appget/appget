using System;
using System.IO;
using System.Text.RegularExpressions;
using AppGet.ProgressTracker;

namespace AppGet.FileTransfer.Protocols
{
    public class WindowsPathFileTransferClient : IFileTransferClient
    {
        private static readonly Regex WindowsPathRegex = new Regex(@"^\\\\|^[a-z]:\\", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public bool CanHandleProtocol(String source)
        {
            return WindowsPathRegex.IsMatch(source);
        }

        public void TransferFile(string source, string destinationFile)
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

        public string ReadString(string source)
        {
            return File.ReadAllText(source);
        }

        public string GetFileName(string source)
        {
            return new FileInfo(source).Name;
        }

        public Action<ProgressState> OnStatusUpdated { get; set; }
        public Action<ProgressState> OnCompleted { get; set; }
    }
}
