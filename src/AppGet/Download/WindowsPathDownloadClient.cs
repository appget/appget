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

        public void DownloadFile(string url, string fileName)
        {
            var progress = new ProgressState
                        {
                            Total = 1
                        };


            File.Copy(url, fileName);

            progress.Completed = 1;
            OnStatusUpdates(progress);
        }

        public Action<ProgressState> OnStatusUpdates { get; set; }
    }
}
