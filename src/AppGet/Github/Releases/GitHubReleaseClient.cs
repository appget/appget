using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AppGet.Http;
using AppGet.Update;
using NLog;

namespace AppGet.Github.Releases
{
    public interface IReleaseClient
    {
        Task<AppGetRelease> GetLatest();
    }

    public class GitHubReleaseClient : IReleaseClient
    {
        private readonly IHttpClient _httpClient;
        private readonly Logger _logger;

        public GitHubReleaseClient(IHttpClient httpClient, Logger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<AppGetRelease> GetLatest()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.github.com/repos/appget/appget/releases/latest");
            request.Headers.Authorization = new AuthenticationHeaderValue("Token","aaf41cf0f1e10a6b6293cce81ef87798aa6d6cbb");
            var response = await _httpClient.SendAsync(request, TimeSpan.FromSeconds(10));
            var releases = await response.Deserialize<GithubRelease>();
            _logger.Trace($"Found {releases.tag_name} AppGet releases");

            return new AppGetRelease
            {
                Url = releases.Assets.Single(a => a.browser_download_url.EndsWith(".exe")).browser_download_url, Version = new Version(releases.tag_name)
            };
        }
    }
}