using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using AppGet.Core.ProgressTracker;

namespace AppGet.Core.Download
{
    public class HttpDownloadClient : IDownloadClient
    {
        private static readonly Regex HttpRegex = new Regex(@"^https?\:\/\/", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private bool _downloading;
        private long _bytesReceived;
        private long _totalBytes;

        private ProgressState _progress;

        public bool CanHandleProtocol(String url)
        {
            return HttpRegex.IsMatch(url);
        }

        public void DownloadFile(string url, string fileName)
        {
            var webClient = new WebClient();
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback);
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadCompletedCallback);
            webClient.DownloadFileAsync(new Uri(url), fileName);

            _downloading = true;

            while (_downloading)
            {
                Thread.Sleep(100);
            }
        }

        private void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
        {
            _progress.Completed = e.BytesReceived;
            _progress.Total = e.TotalBytesToReceive;
            OnStatusUpdates(_progress);
        }

        private void DownloadCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            _downloading = false;
        }

        public Action<ProgressState> OnStatusUpdates { get; set; }
    }
}
