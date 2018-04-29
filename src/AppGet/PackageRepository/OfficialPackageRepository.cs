using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AppGet.Http;
using NLog;

namespace AppGet.PackageRepository
{
    public class OfficialPackageRepository : IPackageRepository
    {
        private readonly IHttpClient _httpClient;
        private readonly Logger _logger;

        private const string API_ROOT = "https://fn.appget.net/api/";

        public OfficialPackageRepository(IHttpClient httpClient, Logger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public PackageInfo Get(string id, string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
            {
                tag = "latest";
            }

            _logger.Info("Getting package " + id);

            try
            {
                var packages = Search(id).ToList();

                var match = packages.FirstOrDefault(c => c.Id == id && c.Tag == tag);

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

        public List<PackageInfo> Search(string term)
        {
            _logger.Debug($"Searching for '{term}' in {API_ROOT}");

            var uri = new Uri($"{API_ROOT}/packages").AddQuery("q", term.Trim());

            var package = _httpClient.Get(uri);

            return package.AsResource<List<PackageInfo>>();
        }
    }
}