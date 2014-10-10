using System.Net;
using AppGet.FlightPlans;
using AppGet.Http;
using AppGet.Serialization;
using NLog;

namespace AppGet.PackageProvider
{
    public class AppGetServerProvider : IPackageProvider
    {
        private readonly IHttpClient _httpClient;
        private readonly Logger _logger;

        public AppGetServerProvider(IHttpClient httpClient, Logger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }


        public FlightPlan GetFlightPlan(string name)
        {
            _logger.Info("Finding package " + name);
            var requestBuilder = new HttpRequestBuilder("http://appget.azurewebsites.net/api/v1/");

            var request = requestBuilder.Build("packages/{package}/latest");
            request.AddSegment("package", name);

            var package = _httpClient.Get<PackageInfo>(request);

            _logger.Info("Downloading flighplan for " + name);
            var response = _httpClient.Get(new HttpRequest(package.Resource.FlightPlanUrl));

            return Yaml.Deserialize<FlightPlan>(response.Content);
        }
    }
}