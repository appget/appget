using System.Net;
using AppGet.FlightPlans;
using AppGet.Http;
using AppGet.Serialization;

namespace AppGet.PackageProvider
{
    public class AppGetServerProvider : IPackageProvider
    {
        private readonly IHttpClient _httpClient;

        public AppGetServerProvider(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public FlightPlan GetFlightPlan(string name)
        {
            var requestBuilder = new HttpRequestBuilder("http://appget.azurewebsites.net/api/v1/");

            var request = requestBuilder.Build("flightplans/{package}/latest");
            request.AddSegment("package", name);

            var package = _httpClient.Get<PackageInfo>(request);


            var yaml = new WebClient().DownloadString(package.Resource.Url);

            return Yaml.Deserialize<FlightPlan>(yaml);
        }
    }
}