using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AppGet.Serialization;
using NLog;

namespace AppGet.Update
{
    public interface IReleaseClient
    {
        Task<List<AppGetRelease>> GetReleases();
    }

    public class GitHubReleaseClient : IReleaseClient
    {
        private readonly Logger _logger;

        public GitHubReleaseClient(Logger logger)
        {
            _logger = logger;
        }

        public async Task<List<AppGetRelease>> GetReleases()
        {
            _logger.Trace("Checking for AppGet updates...");
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "AppGet");
            var response = await client.GetAsync("https://api.github.com/repos/appget/appget/releases?no_cache=" + Guid.NewGuid());
            //            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            var releases = Json.Deserialize<List<GithubRelease>>(responseBody);
            _logger.Trace($"Found {releases.Count} releases");


            return releases.Select(c => new AppGetRelease
            {
                Url = c.Assets.Single(a => a.browser_download_url.EndsWith(".exe")).browser_download_url,
                Version = new Version(c.tag_name)
            }).ToList();

            //            _logger.Trace("Getting AppGet client releases from github");
            //            var builder = new HttpRequestBuilder("https://api.github.com/repos/appget/appget/");
            //            var response = _httpClient.Get<List<GithubRelease>>(builder.Build("releases"));

            //            return releases;
        }
    }
}