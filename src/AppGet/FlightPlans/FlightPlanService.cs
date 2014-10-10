using AppGet.FileTransfer;
using AppGet.PackageRepository;
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
        private readonly IFileTransferService _fileTransferService;
        private readonly Logger _logger;

        public FlightPlanService(IFileTransferService fileTransferService, Logger logger)
        {
            _fileTransferService = fileTransferService;
            _logger = logger;
        }


        public FlightPlan LoadFlightPlan(PackageInfo packageInfo)
        {
            _logger.Info("Loading flighplan for " + packageInfo.Name);
            var text = _fileTransferService.ReadContent(packageInfo.FlightPlanUrl);
            return Yaml.Deserialize<FlightPlan>(text);
        }
    }
}