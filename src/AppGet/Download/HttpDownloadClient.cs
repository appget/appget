using System;
using System.ComponentModel;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using AppGet.Http;
using AppGet.ProgressTracker;

namespace AppGet.Download
{
    public class HttpDownloadClient : IDownloadClient
    {
        private readonly IHttpClient _httpClient;
        private static readonly Regex HttpRegex = new Regex(@"^https?\:\/\/", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private bool _downloading;

        private ProgressState _progress;
        private Exception _error;


        public HttpDownloadClient(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public bool CanHandleProtocol(String source)
        {
            return HttpRegex.IsMatch(source);
        }

        public void DownloadFile(string source, string destination)
        {
            var webClient = new WebClient();
            webClient.DownloadProgressChanged += DownloadProgressCallback;
            webClient.DownloadFileCompleted += DownloadCompletedCallback;
            webClient.DownloadFileAsync(new Uri(source), destination);

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

        public string ReadString(string source)
        {
            var resp = _httpClient.Get(new HttpRequest(source));
            return resp.Content;
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

            if (OnCompleted != null)
            {
                OnCompleted(_progress);
            }
        }

        public Action<ProgressState> OnStatusUpdates { get; set; }
        public Action<ProgressState> OnCompleted { get; set; }
    }
}
