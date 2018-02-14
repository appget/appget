using System.Collections.Generic;
using AppGet.Http;
using NLog;

namespace AppGet.Update
{
    public class GitHubReleaseClient
    {
        private readonly IHttpClient _httpClient;
        private readonly Logger _logger;

        public GitHubReleaseClient(IHttpClient httpClient, Logger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public List<GithubRelease> GetReleases()
        {
            var builder = new HttpRequestBuilder("https://api.github.com/repos/appget/appget/");
            var c = _httpClient.Get<List<GithubRelease>>(builder.Build("releases"));

            return c.Resource;
        }
    }
}