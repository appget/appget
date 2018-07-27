using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace AppGet.Http
{
    public interface IHttpClient
    {
        HttpResponseMessage Send(HttpRequestMessage request, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead);
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead);
        HttpResponseMessage Get(Uri uri, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead);
        Task<HttpResponseMessage> GetAsync(Uri uri, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead);
    }

    public class HttpClient : IHttpClient
    {
        private readonly IUserAgentBuilder _userAgentBuilder;
        private readonly System.Net.Http.HttpClient _client;

        public HttpClient(IUserAgentBuilder userAgentBuilder)
        {
            ServicePointManager.DefaultConnectionLimit = 12;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            _userAgentBuilder = userAgentBuilder;

            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = true,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            _client = new System.Net.Http.HttpClient(handler);
            _client.DefaultRequestHeaders.Add("User-Agent", userAgentBuilder.GetUserAgent(true));
            _client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
        }

        public HttpResponseMessage Send(HttpRequestMessage request, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead)
        {
            try
            {
                return SendAsync(request, completionOption).Result;
            }
            catch (AggregateException ex)
            {
                throw ex.Flatten().InnerExceptions.First();
            }
        }


        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead)
        {
            if (request.RequestUri.Host.EndsWith(".appget.net"))
            {
                request.Headers.Add("User-Agent", _userAgentBuilder.GetUserAgent());
            }

            var response = await _client.SendAsync(request, completionOption);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpException(response);
            }

            return response;
        }

        public HttpResponseMessage Get(Uri uri, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead)
        {
            return Send(new HttpRequestMessage(HttpMethod.Get, uri), completionOption);
        }

        public Task<HttpResponseMessage> GetAsync(Uri uri, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead)
        {
            return SendAsync(new HttpRequestMessage(HttpMethod.Get, uri), completionOption);
        }
    }
}