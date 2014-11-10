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

        public void TransferFile(string source, string destination)
        {
            var progress = new ProgressState
                        {
                            Total = 1
                        };

            //TODO: move this to a Copy using streams/that way we can provide progress
            File.Copy(source, destination);

            progress.Completed = 1;
            
            if (OnStatusUpdated != null)
            {
                OnStatusUpdated(progress);
            }

            if (OnCompleted != null)
            {
                OnCompleted(progress);
            }
        }

        public string ReadString(string source)
        {
            return File.ReadAllText(source);
        }

        public Action<ProgressState> OnStatusUpdated { get; set; }
        public Action<ProgressState> OnCompleted { get; set; }
    }
}
