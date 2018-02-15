using System.Collections.Generic;
using System.Net;
using AppGet.Http;
using NLog;

namespace AppGet.PackageRepository
{
    public class HttpPackageRepository : IPackageRepository
    {
        private readonly IHttpClient _httpClient;
        private readonly Logger _logger;
        private readonly HttpRequestBuilder _requestBuilder;

        private const string API_ROOT = "https://api.appget.net/v1/";

        public HttpPackageRepository(IHttpClient httpClient, Logger logger)
        {
            _httpClient = httpClient;
            _logger = logger;

            _requestBuilder = new HttpRequestBuilder(API_ROOT);
        }


        public PackageInfo GetLatest(string name)
        {
            _logger.Info("Getting package " + name);

            var request = _requestBuilder.Build("packages/{package}/latest");
            request.AddSegment("package", name);

            try
            {
                var package = _httpClient.Get<PackageInfo>(request);
                return package.Resource;
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
            _logger.Info("Searching for " + term);

            var request = _requestBuilder.Build("packages");

            request.UriBuilder.SetQueryParam("q", term.Trim());

            var package = _httpClient.Get<List<PackageInfo>>(request);
            return package.Resource;
        }
    }
}