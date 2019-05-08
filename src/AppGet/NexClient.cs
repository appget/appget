using System;
using System.Net.Http;
using System.Threading.Tasks;
using AppGet.Http;
using AppGet.Installers;
using NLog;

namespace AppGet
{
    public class NexClient
    {
        private readonly IHttpClient _httpClient;
        private readonly Logger _logger;

        private const string API_ROOT = "https://nex.appget.net";

        public NexClient(IHttpClient httpClient, Logger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }


        public async Task SubmitReport(InstallerReport report)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, $"{API_ROOT}/install")
                {
                    Content = new JsonContent(report)
                };

                await _httpClient.SendAsync(request, TimeSpan.FromSeconds(5));
            }
            catch (Exception e)
            {
                _logger.Trace(e, "Couldn't submit installation report.");
            }
        }
    }
}
