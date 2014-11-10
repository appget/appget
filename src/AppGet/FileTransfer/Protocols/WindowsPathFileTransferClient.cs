using System;
using System.IO;
using System.Text.RegularExpressions;
using AppGet.FileSystem;
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

        public string TransferFile(string source, string destinationDirectory)
        {
            var progress = new ProgressState
                        {
                            Total = 1
                        };

            var filePath = Path.Combine(destinationDirectory, GetFileName(source));

            //TODO: move this to a Copy using streams/that way we can provide progress
            File.Copy(source, destinationDirectory);

            progress.Completed = 1;

            if (OnStatusUpdated != null)
            {
                OnStatusUpdated(progress);
            }

            if (OnCompleted != null)
            {
                OnCompleted(progress);
            }

            return filePath;
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
