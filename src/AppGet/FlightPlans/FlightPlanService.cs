using AppGet.FileTransfer;
using AppGet.PackageRepository;
using AppGet.Serialization;
using NLog;

namespace AppGet.FlightPlans
{
    public interface IFlightPlanService
    {
        FlightPlan LoadFlightPlan(PackageInfo packageInfo);
        string ReadFlightPlan(PackageInfo packageInfo);
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
            var text = ReadFlightPlan(packageInfo);
            return Yaml.Deserialize<FlightPlan>(text);
        }

        public string ReadFlightPlan(PackageInfo packageInfo)
        {
            _logger.Info("Loading flighplan for " + packageInfo);
            var text = _fileTransferService.ReadContent(packageInfo.FlightPlanUrl);
            return text;
        }
    }
}