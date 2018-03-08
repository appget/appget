using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AppGet.FileSystem;
using AppGet.Http;
using AppGet.ProgressTracker;

namespace AppGet.FileTransfer.Protocols
{
    public class HttpFileTransferClient : IFileTransferClient
    {
        private readonly IHttpClient _httpClient;
        private readonly IFileSystem _fileSystem;
        private static readonly Regex HttpRegex = new Regex(@"^https?\:\/\/", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex FileNameRegex = new Regex(@"\.(zip|7zip|7z|rar|msi|exe)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private bool _inTransit;

        private ProgressState _progress;
        private Exception _error;


        public HttpFileTransferClient(IHttpClient httpClient, IFileSystem fileSystem)
        {
            _httpClient = httpClient;
            _fileSystem = fileSystem;
        }

        public bool CanHandleProtocol(string source)
        {
            return HttpRegex.IsMatch(source);
        }


        public async Task<string> GetFileName(string source)
        {
            var uri = new Uri(source);

            var fileName = Path.GetFileName(uri.LocalPath);

            if (FileNameRegex.IsMatch(fileName))
            {
                return fileName;
            }

            var resp = await _httpClient.Head(uri);

            if (resp.RequestMessage.RequestUri != uri)
            {
                return await GetFileName(resp.RequestMessage.RequestUri.ToString());
            }

            if (resp.Content.Headers.ContentDisposition != null)
            {
                return resp.Content.Headers.ContentDisposition.FileName;
            }

            throw new InvalidDownloadUrlException(source);

        }

        public void TransferFile(string source, string destinationFile)
        {
            _error = null;
            string tempFile;
            using (var webClient = new WebClientWithTimeout(TimeSpan.FromSeconds(1)))
            {
                if (_fileSystem.FileExists(destinationFile))
                {
                    _fileSystem.DeleteFile(destinationFile);
                }

                tempFile = destinationFile + ".APPGET_DOWNLOAD";

                webClient.DownloadProgressChanged += TransferProgressCallback;
                webClient.DownloadFileCompleted += TransferCompletedCallback;
                webClient.DownloadFileAsync(new Uri(source), tempFile);


                _inTransit = true;
                _progress = new ProgressState();

                while (_inTransit)
                {
                    Thread.Sleep(100);
                }

                if (_error != null)
                {
                    if (_fileSystem.FileExists(tempFile))
                    {
                        _fileSystem.DeleteFile(tempFile);
                    }

                    var e = _error;
                    _error = null;
                    throw e;
                }
            }


            _fileSystem.Move(tempFile, destinationFile);
            OnCompleted?.Invoke(_progress);
        }

        public async Task<string> ReadString(string source)
        {
            var req = new HttpRequestMessage(HttpMethod.Get, source);
            req.Headers.CacheControl = new CacheControlHeaderValue { NoCache = true, NoStore = true };

            var resp = await _httpClient.Send(req);
            return await resp.Content.ReadAsStringAsync();
        }

        private void TransferProgressCallback(object sender, DownloadProgressChangedEventArgs e)
        {
            _progress.Completed = e.BytesReceived;

            var client = (WebClient)sender;

            var contentType = client.ResponseHeaders["Content-Type"];
            if (contentType != null && contentType.Contains("text"))
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
        }

        public Action<ProgressState> OnStatusUpdated { get; set; }
        public Action<ProgressState> OnCompleted { get; set; }
    }
}
