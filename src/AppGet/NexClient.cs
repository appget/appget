using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AppGet.Http;
using AppGet.Installers;
using AppGet.PackageRepository;
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

        public async Task<Repository> AuthenticateRepository(string repoId, string key)
        {
            _logger.Info("Requesting access to repository {0}", repoId);

            var request = new HttpRequestMessage(HttpMethod.Post, $"{API_ROOT}/repos/{repoId}/auth");
            request.Headers.Add("X-Repo-Key", key);

            var resp = await _httpClient.SendAsync(request, TimeSpan.FromMinutes(1));
            var repo = await resp.Deserialize<Repository>();

            _logger.Info("Successfully authenticated to {0}", repo.Name);

            return repo;
        }
    }
}
