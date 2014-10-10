using AppGet.Commands.Install;
using AppGet.Compression;
using AppGet.Download;
using AppGet.FlightPlans;
using NLog;

namespace AppGet.Install
{
    public class InstallService
    {
        private readonly Logger _logger;
        private readonly ZipInstaller _zipInstaller;

        public InstallService(Logger logger, ZipInstaller zipInstaller)
        {
            _logger = logger;
            _zipInstaller = zipInstaller;
        }

        public void Install(FlightPlan flightPlan, InstallOptions installOptions)
        {
            _zipInstaller.Install(flightPlan, installOptions);
        }

    }


    public class ZipInstaller
    {
        private readonly Logger _logger;
        private readonly IDownloadService _downloadService;

        public ZipInstaller(Logger logger, CompressionService compressionService, IDownloadService downloadService)
        {
            _logger = logger;
            _downloadService = downloadService;
        }

        public void Install(FlightPlan flightPlan, InstallOptions installOptions)
        {

        }
    }
}
