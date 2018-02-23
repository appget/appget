using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
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


        public async Task<PackageInfo> GetLatest(string name)
        {
            _logger.Info("Getting package " + name);

            try
            {
                var package = await _httpClient.Get($"{API_ROOT}/packages/{name}/latest");
                return package.AsResource<PackageInfo>();
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
            _logger.Info("Searching for " + term);

            var uri = new Uri($"{API_ROOT}/packages")
                .AddQuery("q", term.Trim());

            var package = await _httpClient.Get(uri);
            return package.AsResource<List<PackageInfo>>();
        }
    }
}