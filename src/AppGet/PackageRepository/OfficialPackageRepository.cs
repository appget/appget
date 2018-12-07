using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AppGet.Http;
using AppGet.Manifest;
using NLog;

namespace AppGet.PackageRepository
{
    public class OfficialPackageRepository : IPackageRepository
    {
        private readonly IHttpClient _httpClient;
        private readonly RepositoryRegistry _repositoryRegistry;
        private readonly Logger _logger;

        private const string API_ROOT = "https://nex.appget.net";

        public OfficialPackageRepository(IHttpClient httpClient, RepositoryRegistry repositoryRegistry, Logger logger)
        {
            _httpClient = httpClient;
            _repositoryRegistry = repositoryRegistry;
            _logger = logger;
        }

        public async Task<PackageInfo> GetAsync(string id, string tag, string repository)
        {
            _logger.Info($"Getting package {id}:{tag ?? PackageManifest.LATEST_TAG}");

            try
            {
                var term = $"{id}";

                if (!string.IsNullOrWhiteSpace(tag))
                {
                    term += $":{tag}";
                }

                var packages = await Search(term, repository, true);

                var match = packages.FirstOrDefault(c => c.Selected);

                if (match != null)
                {
                    return match;
                }

                throw new PackageNotFoundException(id, tag, packages);
            }
            catch (HttpException ex)
            {
                if (ex.Response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                throw;
            }
        }

        public async Task<List<PackageInfo>> Search(string term, string repository = null, bool @select = false)
        {
            _logger.Debug($"Searching for '{term}' in {API_ROOT}");

            var url = $"{API_ROOT}/packages";

            if (repository != null)
            {
                var repo = _repositoryRegistry.Get(repository);
                url += $"/{repo.RepoId}";
            }

            url += $"?q={term}";


            if (select)
            {
                url += "&s=1";
            }


            var package = await _httpClient.GetAsync(new Uri(url), TimeSpan.FromSeconds(30));

            return await package.Deserialize<List<PackageInfo>>();
        }
    }
}
