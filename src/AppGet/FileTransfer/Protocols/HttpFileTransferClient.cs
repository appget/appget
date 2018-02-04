using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using AppGet.Exceptions;
using AppGet.Http;
using AppGet.ProgressTracker;

namespace AppGet.FileTransfer.Protocols
{
    public class HttpFileTransferClient : IFileTransferClient
    {
        private readonly IHttpClient _httpClient;
        private static readonly Regex HttpRegex = new Regex(@"^https?\:\/\/", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex FileNameRegex = new Regex(@"\w\.(zip|msi|exe|7zip|rar)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex ContentDepositionRegex = new Regex(@"filename=\W*(?<fileName>.+\.\w{2,4})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
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


        public string GetFileName(string source)
        {
            var url = new Uri(source);

            var fileName = url.LocalPath.Split('/').Last();

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

            throw new AppGetException("Couldn't get file name from " + source);

        }

        public void TransferFile(string source, string destinationFile)
        {
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

            OnStatusUpdated?.Invoke(_progress);
        }

        private void TransferCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            _error = e.Error;
            _inTransit = false;

            OnCompleted?.Invoke(_progress);
        }

        public Action<ProgressState> OnStatusUpdated { get; set; }
        public Action<ProgressState> OnCompleted { get; set; }
    }
}
