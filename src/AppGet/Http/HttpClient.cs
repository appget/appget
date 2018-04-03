using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace AppGet.Http
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> Send(HttpRequestMessage request, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead);
        Task<HttpResponseMessage> Get(Uri uri, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead);
    }

    public class HttpClient : IHttpClient
    {
        private readonly IUserAgentBuilder _userAgentBuilder;
        private readonly System.Net.Http.HttpClient _client;

        public HttpClient(IUserAgentBuilder userAgentBuilder)
        {
            ServicePointManager.DefaultConnectionLimit = 12;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol |=
                SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            _userAgentBuilder = userAgentBuilder;

            _client = new System.Net.Http.HttpClient();
            _client.DefaultRequestHeaders.Add("User-Agent", userAgentBuilder.GetUserAgent(true));
        }

        public async Task<HttpResponseMessage> Send(HttpRequestMessage request, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead)
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

        public async Task<HttpResponseMessage> Get(Uri uri, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead)
        {
            return await Send(new HttpRequestMessage(HttpMethod.Get, uri), completionOption);
        }
    }
}