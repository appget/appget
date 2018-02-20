using System;
using System.ComponentModel;
using System.Linq;
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
        private static readonly Regex FileNameRegex = new Regex(@"\w\.(zip|7zip|7z|rar|msi|exe)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex ContentDepositionRegex = new Regex(@"filename=\W*(?<fileName>.+\.\w{2,4})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private bool _inTransit;

        private ProgressState _progress;
        private Exception _error;


        public HttpFileTransferClient(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public bool CanHandleProtocol(string source)
        {
            return HttpRegex.IsMatch(source);
        }


        public string GetFileName(string source)
        {
            var url = new Uri(source);

            var fileName = url.Segments.Last();

            if (FileNameRegex.IsMatch(fileName))
            {
                return fileName;
            }

            var resp = _httpClient.Head(new HttpRequest(source) { AllowAutoRedirect = false });

            if (resp.Headers.ContainsKey("Location"))
            {
                return GetFileName(resp.Headers["Location"].ToString());
            }

            if (resp.Headers.ContainsKey("Content-Disposition"))
            {
                var contentDeposition = resp.Headers["Content-Disposition"].ToString();

                var match = ContentDepositionRegex.Match(contentDeposition);

                if (match.Success && match.Groups["fileName"] != null)
                {
                    return match.Groups["fileName"].Value;
                }
            }

            throw new InvalidDownloadUrlException(source);

        }

        public void TransferFile(string source, string destinationFile)
        {
            _error = null;
            var webClient = new WebClient();

            webClient.DownloadProgressChanged += TransferProgressCallback;
            webClient.DownloadFileCompleted += TransferCompletedCallback;
            webClient.DownloadFileAsync(new Uri(source), destinationFile);

            _inTransit = true;
            _progress = new ProgressState();

            while (_inTransit)
            {
                Thread.Sleep(100);
            }

            if (_error != null)
            {
                var e = _error;
                _error = null;
                throw e;
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
            var client = (WebClient)sender;
            var contentType = client.ResponseHeaders["Content-Type"];
            _progress.Completed = e.BytesReceived;



            if (contentType.Contains("text"))
            {
                _error = new InvalidDownloadUrlException(client.BaseAddress, $"[ContentType={contentType}]");
                client.CancelAsync();
            }

            if (e.TotalBytesToReceive > 0)
            {
                _progress.Total = e.TotalBytesToReceive;
            }
            else
            {
                _progress.Total = null;
            }

            OnStatusUpdated?.Invoke(_progress);
        }

        private void TransferCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            var webClient = (WebClient)sender;
            webClient.DownloadProgressChanged -= TransferProgressCallback;
            _inTransit = false;

            if (_error == null)
            {
                _error = e.Error;
            }

            OnCompleted?.Invoke(_progress);
        }

        public Action<ProgressState> OnStatusUpdated { get; set; }
        public Action<ProgressState> OnCompleted { get; set; }
    }
}
