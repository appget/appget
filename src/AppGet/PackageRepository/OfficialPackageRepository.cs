using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AppGet.Http;
using NLog;

namespace AppGet.PackageRepository
{
    public class OfficialPackageRepository : IPackageRepository
    {
        private readonly IHttpClient _httpClient;
        private readonly Logger _logger;

        private const string API_ROOT = "https://api.appget.net/v1/";

        public OfficialPackageRepository(IHttpClient httpClient, Logger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }


        public async Task<PackageInfo> Get(string id, string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
            {
                tag = "latest";
            }

            _logger.Info("Getting package " + id);

            try
            {
                var package = await _httpClient.Get($"{API_ROOT}/packages/{id}/{tag}");
                return await package.AsResource<PackageInfo>();
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

        public async Task<List<PackageInfo>> Search(string term)
        {
            _logger.Debug($"Searching for '{term}' in {API_ROOT}");

            var uri = new Uri($"{API_ROOT}/packages")
                .AddQuery("q", term.Trim());

            var package = await _httpClient.Get(uri);
            return await package.AsResource<List<PackageInfo>>();
        }
    }
}