using System;
using System.Net;
using System.Net.Http;

namespace AppGet.Http
{
    public interface IHttpClient
    {
        HttpResponseMessage Send(HttpRequestMessage request, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead);

        HttpResponseMessage Get(Uri uri, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead);
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

            _client = new System.Net.Http.HttpClient();
            _client.DefaultRequestHeaders.Add("User-Agent", userAgentBuilder.GetUserAgent(true));
        }

        public HttpResponseMessage Send(HttpRequestMessage request, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead)
        {
            if (request.RequestUri.Host.EndsWith(".appget.net"))
            {
                request.Headers.Add("User-Agent", _userAgentBuilder.GetUserAgent());
            }

            var response = _client.SendAsync(request, completionOption).Result;

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
    }
}