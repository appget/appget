using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AppGet.Http;
using AppGet.Manifests;
using NLog;

namespace AppGet.PackageRepository
{
    public class OfficialPackageRepository : IPackageRepository
    {
        private readonly IHttpClient _httpClient;
        private readonly Logger _logger;

        private const string API_ROOT = "https://fn.appget.net/api";

        public OfficialPackageRepository(IHttpClient httpClient, Logger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public PackageInfo Get(string id, string tag)
        {
            _logger.Info($"Getting package {id}:{tag ?? PackageManifest.LATEST_TAG}");

            try
            {
                var term = $"{id}";

                if (!string.IsNullOrWhiteSpace(tag))
                {
                    term += $":{tag}";
                }

                var packages = Search(term, true).ToList();

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

        public List<PackageInfo> Search(string term, bool select = false)
        {
            _logger.Debug($"Searching for '{term}' in {API_ROOT}");

            var uri = new Uri($"{API_ROOT}/packages?q={term}");

            if (select)
            {
                uri = new Uri($"{uri}&s=1");
            }

            var package = _httpClient.Get(uri);

            return package.AsResource<List<PackageInfo>>();
        }
    }
}