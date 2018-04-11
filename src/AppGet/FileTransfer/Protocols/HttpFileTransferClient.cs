using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading;
using AppGet.FileSystem;
using AppGet.Http;
using AppGet.ProgressTracker;

namespace AppGet.FileTransfer.Protocols
{
    public class HttpFileTransferClient : IFileTransferClient
    {
        private static readonly Regex HttpRegex = new Regex(@"^https?\:\/\/", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex FileNameRegex = new Regex(@"\.(zip|7zip|7z|rar|msi|exe)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Dictionary<string, WebHeaderCollection> HeaderCache = new Dictionary<string, WebHeaderCollection>();

        private readonly IFileSystem _fileSystem;
        private readonly IHttpClient _httpClient;

        public HttpFileTransferClient(IHttpClient httpClient, IFileSystem fileSystem)
        {
            _httpClient = httpClient;
            _fileSystem = fileSystem;
        }

        public bool CanHandleProtocol(string source)
        {
            return HttpRegex.IsMatch(source);
        }

        public string GetFileName(string source)
        {
            var uri = new Uri(source);

            var fileName = Path.GetFileName(uri.LocalPath);

            if (FileNameRegex.IsMatch(fileName)) return fileName;

            var resp = _httpClient.Get(uri, HttpCompletionOption.ResponseHeadersRead);

            if (resp.RequestMessage.RequestUri != uri) return GetFileName(resp.RequestMessage.RequestUri.ToString());

            if (resp.Content.Headers.ContentDisposition != null) return resp.Content.Headers.ContentDisposition.FileName.Trim('"', '\'', ' ');

            throw new InvalidDownloadUrlException(source);
        }

        public void TransferFile(string source, string destinationFile)
        {
            Exception error = null;
            var tempFile = $"{destinationFile}.APPGET_DOWNLOAD";
            var progress = new ProgressState();

            using (var webClient = new WebClientWithTimeout(TimeSpan.FromSeconds(10)))
            {
                if (_fileSystem.FileExists(destinationFile)) _fileSystem.DeleteFile(destinationFile);

                webClient.DownloadProgressChanged += (sender, e) =>
                {
                    progress.Completed = e.BytesReceived;

                    if (e.TotalBytesToReceive > 0) progress.Total = e.TotalBytesToReceive;
                    else progress.Total = null;
                };

                webClient.DownloadFileCompleted += (sender, e) =>
                {
                    if (error == null) error = e.Error;

                    if (error != null) return;

                    var client = (WebClient)sender;

                    var contentType = client.ResponseHeaders["Content-Type"];
                    if (contentType != null && contentType.Contains("text"))
                        error = new InvalidDownloadUrlException(client.BaseAddress, $"[ContentType={contentType}]");
                };

                webClient.DownloadFileAsync(new Uri(source), tempFile);

                while (webClient.IsBusy)
                {
                    Thread.Sleep(200);
                    OnStatusUpdated?.Invoke(progress);
                }

                if (error != null)
                {
                    if (_fileSystem.FileExists(tempFile)) _fileSystem.DeleteFile(tempFile);

                    throw error;
                }

                HeaderCache[source] = webClient.ResponseHeaders;
            }

            OnCompleted?.Invoke(progress);
            _fileSystem.Move(tempFile, destinationFile);
        }

        public string ReadString(string source)
        {
            var req = new HttpRequestMessage(HttpMethod.Get, source);
            req.Headers.CacheControl = new CacheControlHeaderValue
            {
                NoCache = true,
                NoStore = true
            };

            var resp = _httpClient.Send(req);

            return resp.Content.ReadAsStringAsync().Result;
        }

        public Action<ProgressState> OnStatusUpdated { get; set; }
        public Action<ProgressState> OnCompleted { get; set; }

        public static WebHeaderCollection GetTransferHeaders(string url)
        {
            return HeaderCache.ContainsKey(url) ? HeaderCache[url] : null;
        }
    }
}