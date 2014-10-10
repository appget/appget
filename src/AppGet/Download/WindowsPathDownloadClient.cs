using System;
using System.IO;
using System.Text.RegularExpressions;
using AppGet.ProgressTracker;

namespace AppGet.Download
{
    public class WindowsPathDownloadClient : IDownloadClient
    {
        private static readonly Regex WindowsPathRegex = new Regex(@"^\\\\|^[a-z]:\\", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public bool CanHandleProtocol(String url)
        {
            return WindowsPathRegex.IsMatch(url);
        }

        public void DownloadFile(string url, string destination)
        {
            var progress = new ProgressState
                        {
                            Total = 1
                        };

            //TODO: move this to a Copy using streams/that way we can provide progress
            File.Copy(url, destination);

            progress.Completed = 1;
            
            if (OnStatusUpdates != null)
            {
                OnStatusUpdates(progress);
            }

            if (OnCompleted != null)
            {
                OnCompleted(progress);
            }
        }

        public Action<ProgressState> OnStatusUpdates { get; set; }
        public Action<ProgressState> OnCompleted { get; set; }
    }
}
