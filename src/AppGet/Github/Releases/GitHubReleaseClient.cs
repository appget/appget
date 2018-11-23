using System;
using System.Linq;
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
            var uri = new Uri($"https://api.github.com/repos/appget/appget/releases/latest?{GithubKeys.AuthQuery}&no_cache={Guid.NewGuid()}");
            var response = await _httpClient.GetAsync(uri, TimeSpan.FromSeconds(10));
            var releases = await response.Deserialize<GithubRelease>();
            _logger.Trace($"Found {releases.tag_name} AppGet releases");

            return new AppGetRelease
            {
                Url = releases.Assets.Single(a => a.browser_download_url.EndsWith(".exe")).browser_download_url,
                Version = new Version(releases.tag_name)
            };
        }
    }
}