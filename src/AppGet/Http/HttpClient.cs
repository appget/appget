using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using NLog;

namespace AppGet.Http
{
    public interface IHttpClient
    {
        HttpResponse Execute(HttpRequest request);
        HttpResponse Get(HttpRequest request);
        HttpResponse<T> Get<T>(HttpRequest request) where T : new();
        HttpResponse Head(HttpRequest request);
    }

    public class HttpClient : IHttpClient
    {
        private readonly Logger _logger;
        private readonly string _userAgent;

        public HttpClient(Logger logger)
        {
            _logger = logger;
            _userAgent = String.Format("AppGet Client");
            ServicePointManager.DefaultConnectionLimit = 12;
        }

        public HttpResponse Execute(HttpRequest request)
        {
            _logger.Debug(request);

            var webRequest = (HttpWebRequest)WebRequest.Create(request.Url);

            // Deflate is not a standard and could break depending on implementation.
            // we should just stick with the more compatible Gzip
            //http://stackoverflow.com/questions/8490718/how-to-decompress-stream-deflated-with-java-util-zip-deflater-in-net
            webRequest.AutomaticDecompression = DecompressionMethods.GZip;

            webRequest.Credentials = request.NetworkCredential;
            webRequest.Method = request.Method.ToString();
            webRequest.UserAgent = _userAgent;
            webRequest.KeepAlive = false;
            webRequest.AllowAutoRedirect = request.AllowAutoRedirect;


            var stopWatch = Stopwatch.StartNew();

            if (request.Headers != null)
            {
                AddRequestHeaders(webRequest, request.Headers);
            }

            if (!String.IsNullOrEmpty(request.Body))
            {
                var bytes = new byte[request.Body.Length * sizeof(char)];
                Buffer.BlockCopy(request.Body.ToCharArray(), 0, bytes, 0, bytes.Length);

                webRequest.ContentLength = bytes.Length;
                using (var writeStream = webRequest.GetRequestStream())
                {
                    writeStream.Write(bytes, 0, bytes.Length);
                }
            }

            HttpWebResponse httpWebResponse;

            try
            {
                httpWebResponse = (HttpWebResponse)webRequest.GetResponse();
            }
            catch (WebException e)
            {
                httpWebResponse = (HttpWebResponse)e.Response;

                if (httpWebResponse == null)
                {
                    throw;
                }
            }

            string data = null;

            using (var responseStream = httpWebResponse.GetResponseStream())
            {
                if (responseStream != null)
                {
                    var reader = new StreamReader(responseStream);
                    data = reader.ReadToEnd();
                }
            }

            stopWatch.Stop();

            var response = new HttpResponse(request, new HttpHeader(httpWebResponse.Headers), data, httpWebResponse.StatusCode);
            _logger.Debug("{0} ({1:n0} ms)", response, stopWatch.ElapsedMilliseconds);

            if (response.HasHttpError)
            {
                _logger.Debug("HTTP Error - {0}", response);
                throw new HttpException(request, response);
            }

            return response;
        }

        public HttpResponse Get(HttpRequest request)
        {
            request.Method = HttpMethod.GET;
            return Execute(request);
        }

        public HttpResponse<T> Get<T>(HttpRequest request) where T : new()
        {
            var response = Get(request);
            return new HttpResponse<T>(response);
        }

        public HttpResponse Head(HttpRequest request)
        {
            request.Method = HttpMethod.HEAD;
            return Execute(request);
        }

        protected virtual void AddRequestHeaders(HttpWebRequest webRequest, HttpHeader headers)
        {
            foreach (var header in headers)
            {
                switch (header.Key)
                {
                    case "Accept":
                        webRequest.Accept = header.Value.ToString();
                        break;
                    case "Connection":
                        webRequest.Connection = header.Value.ToString();
                        break;
                    case "Content-Length":
                        webRequest.ContentLength = Convert.ToInt64(header.Value);
                        break;
                    case "Content-Type":
                        webRequest.ContentType = header.Value.ToString();
                        break;
                    case "Expect":
                        webRequest.Expect = header.Value.ToString();
                        break;
                    case "If-Modified-Since":
                        webRequest.IfModifiedSince = (DateTime)header.Value;
                        break;
                    case "Range":
                        throw new NotImplementedException();
                    case "Referer":
                        webRequest.Referer = header.Value.ToString();
                        break;
                    case "Transfer-Encoding":
                        webRequest.TransferEncoding = header.Value.ToString();
                        break;
                    case "User-Agent":
                        throw new NotSupportedException("User-Agent other than NzbDrone not allowed.");
                    case "Proxy-Connection":
                        throw new NotImplementedException();
                    default:
                        webRequest.Headers.Add(header.Key, header.Value.ToString());
                        break;
                }
            }
        }
    }
}