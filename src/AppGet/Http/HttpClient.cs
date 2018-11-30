using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AppGet.HostSystem;

namespace AppGet.Http
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, TimeSpan timeout, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead);
        Task<HttpResponseMessage> GetAsync(Uri uri, TimeSpan timeout, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead);
    }

    public class HttpClient : IHttpClient
    {
        private readonly IUserAgentBuilder _userAgentBuilder;
        private readonly MachineId _machineId;
        private readonly HttpClientHandler _handler;

        private System.Net.Http.HttpClient GetClient(TimeSpan timeout)
        {
            var client = new System.Net.Http.HttpClient(_handler);
            client.DefaultRequestHeaders.Add("User-Agent", _userAgentBuilder.GetUserAgent(true));
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
            client.Timeout = timeout;

            return client;
        }

        public HttpClient(IUserAgentBuilder userAgentBuilder, MachineId machineId)
        {
            ServicePointManager.DefaultConnectionLimit = 12;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            _userAgentBuilder = userAgentBuilder;
            _machineId = machineId;

            _handler = new HttpClientHandler
            {
                AllowAutoRedirect = true,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
        }


        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, TimeSpan timeout, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead)
        {
            if (request.RequestUri.Host.EndsWith(".appget.net") || request.RequestUri.Host == "localhost")
            {
                request.Headers.Add("User-Agent", _userAgentBuilder.GetUserAgent());
                request.Headers.Add("X-Client-Key", _machineId.MachineKey.Value);
            }

            try
            {
                var client = GetClient(timeout);
                var response = await client.SendAsync(request, completionOption);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpException(response);
                }

                return response;
            }
            catch (TaskCanceledException)
            {
                throw new HttpTimeoutException(request);
            }
            catch (HttpRequestException requestException)
            {
                if (requestException.InnerException != null)
                {
                    throw requestException.InnerException;
                }

                throw;
            }
        }

        public Task<HttpResponseMessage> GetAsync(Uri uri, TimeSpan timeout, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead)
        {
            return SendAsync(new HttpRequestMessage(HttpMethod.Get, uri), timeout, completionOption);
        }
    }
}