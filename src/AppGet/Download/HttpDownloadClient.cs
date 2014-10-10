using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using AppGet.ProgressTracker;

namespace AppGet.Download
{
    public class HttpDownloadClient : IDownloadClient
    {
        private static readonly Regex HttpRegex = new Regex(@"^https?\:\/\/", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private bool _downloading;

        private ProgressState _progress;
        private Exception _error;

        public bool CanHandleProtocol(String url)
        {
            return HttpRegex.IsMatch(url);
        }

        public void DownloadFile(string url, string destination)
        {
            var webClient = new WebClient();
            webClient.DownloadProgressChanged += DownloadProgressCallback;
            webClient.DownloadFileCompleted += DownloadCompletedCallback;
            webClient.DownloadFileAsync(new Uri(url), destination);

            _downloading = true;
            _progress = new ProgressState();

            while (_downloading)
            {
                Thread.Sleep(100);
            }

            if (_error != null)
            {
                throw _error;
            }
        }

        private void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
        {
            _progress.Completed = e.BytesReceived;
            _progress.Total = e.TotalBytesToReceive;

            if (OnStatusUpdates != null)
            {
                OnStatusUpdates(_progress);
            }
        }

        private void DownloadCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            _error = e.Error;
            _downloading = false;
        }

        public Action<ProgressState> OnStatusUpdates { get; set; }
    }
}
