using System.Net;
using AppGet.Http;
using NLog;

namespace AppGet.PackageRepository
{
    public class AppGetServerClient : IPackageRepository
    {
        private readonly IHttpClient _httpClient;
        private readonly Logger _logger;

        private const string API_ROOT = "http://appget.net/api/v1/";

        public AppGetServerClient(IHttpClient httpClient, Logger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }


        public PackageInfo FindPackage(string name)
        {
            _logger.Info("Finding package " + name);
            var requestBuilder = new HttpRequestBuilder(API_ROOT);

            var request = requestBuilder.Build("packages/{package}/latest");
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
    }
}