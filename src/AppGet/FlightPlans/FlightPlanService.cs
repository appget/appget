using AppGet.Download;
using AppGet.Http;
using AppGet.PackageProvider;
using AppGet.Serialization;
using NLog;

namespace AppGet.FlightPlans
{
    public interface IFlightPlanService
    {
        FlightPlan LoadFlightPlan(PackageInfo packageInfo);
    }

    public class FlightPlanService : IFlightPlanService
    {
        private readonly IDownloadService _downloadService;
        private readonly Logger _logger;

        public FlightPlanService(IDownloadService downloadService, Logger logger)
        {
            _downloadService = downloadService;
            _logger = logger;
        }


        public FlightPlan LoadFlightPlan(PackageInfo packageInfo)
        {
            _logger.Info("Loading flighplan for " + packageInfo.Name);
            var text = _downloadService.ReadString(packageInfo.FlightPlanUrl);
            return Yaml.Deserialize<FlightPlan>(text);
        }
    }
}