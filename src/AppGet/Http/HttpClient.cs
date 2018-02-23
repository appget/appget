using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NLog;

namespace AppGet.Http
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> Send(HttpRequestMessage request);
        Task<HttpResponseMessage> Get(string url);
        Task<HttpResponseMessage> Get(Uri uri);
        Task<HttpResponseMessage> Head(string url);
        Task<HttpResponseMessage> Head(Uri uri);
    }

    public class HttpClient : IHttpClient
    {
        private readonly Logger _logger;
        private readonly string _userAgent;
        private readonly System.Net.Http.HttpClient _client;

        public HttpClient(Logger logger)
        {
            ServicePointManager.DefaultConnectionLimit = 12;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol |=
                SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            _logger = logger;
            _userAgent = "AppGet Client";

            _client = new System.Net.Http.HttpClient();
            _client.DefaultRequestHeaders.Add("User-Agent", "AppGet Client");
        }

        public async Task<HttpResponseMessage> Send(HttpRequestMessage request)
        {
            var response = await _client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpException(response);
            }

            return response;
        }


        public async Task<HttpResponseMessage> Get(string url)
        {
            return await Get(new Uri(url));
        }

        public async Task<HttpResponseMessage> Get(Uri uri)
        {
            return await Send(new HttpRequestMessage(HttpMethod.Get, uri));
        }

        public async Task<HttpResponseMessage> Head(string url)
        {
            return await Head(new Uri(url));
        }

        public async Task<HttpResponseMessage> Head(Uri uri)
        {
            return await Send(new HttpRequestMessage(HttpMethod.Head, uri));
        }
    }
}