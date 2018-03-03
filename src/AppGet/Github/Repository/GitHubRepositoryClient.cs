using System.Threading.Tasks;
using AppGet.Http;

namespace AppGet.Github.Repository
{
    public interface IGitHubRepositoryClient
    {
        Task<Repository> Get(string owner, string name);
    }

    public class GitHubRepositoryClient : IGitHubRepositoryClient
    {
        private readonly IHttpClient _httpClient;

        public GitHubRepositoryClient(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Repository> Get(string owner, string name)
        {
            var response = await _httpClient.Get($"https://api.github.com/repos/{owner}/{name}?{GithubKeys.AuthQuery}");
            var repository = await response.AsResource<Repository>();

            return repository;
        }
    }
}