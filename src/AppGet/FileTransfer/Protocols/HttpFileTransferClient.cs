using System;
using System.ComponentModel;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using AppGet.Http;
using AppGet.ProgressTracker;

namespace AppGet.FileTransfer.Protocols
{
    public class HttpFileTransferClient : IFileTransferClient
    {
        private readonly IHttpClient _httpClient;
        private static readonly Regex HttpRegex = new Regex(@"^https?\:\/\/", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private bool _inTransit;

        private ProgressState _progress;
        private Exception _error;


        public HttpFileTransferClient(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public bool CanHandleProtocol(String source)
        {
            return HttpRegex.IsMatch(source);
        }

        public void TransferFile(string source, string destination)
        {
            var webClient = new WebClient();
            webClient.DownloadProgressChanged += TransferProgressCallback;
            webClient.DownloadFileCompleted += TransferCompletedCallback;
            webClient.DownloadFileAsync(new Uri(source), destination);

            _inTransit = true;
            _progress = new ProgressState();

            while (_inTransit)
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
            var req = new HttpRequest(source);
            req.DisableCache();
            var resp = _httpClient.Get(req);
            return resp.Content;
        }

        private void TransferProgressCallback(object sender, DownloadProgressChangedEventArgs e)
        {
            _progress.Completed = e.BytesReceived;
            _progress.Total = e.TotalBytesToReceive;

            if (OnStatusUpdated != null)
            {
                OnStatusUpdated(_progress);
            }
        }

        private void TransferCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            _error = e.Error;
            _inTransit = false;

            if (OnCompleted != null)
            {
                OnCompleted(_progress);
            }
        }

        public Action<ProgressState> OnStatusUpdated { get; set; }
        public Action<ProgressState> OnCompleted { get; set; }
    }
}
