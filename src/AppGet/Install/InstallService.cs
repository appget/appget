using AppGet.Commands.Install;
using AppGet.Compression;
using AppGet.Download;
using AppGet.FlightPlans;
using AppGet.HostSystem;
using NLog;

namespace AppGet.Install
{
    public interface IInstallService
    {
        void Install(string installerLocation, FlightPlan flightPlan, InstallOptions installOptions);
    }

    public class InstallService : IInstallService
    {
        private readonly Logger _logger;
        private readonly ZipInstaller _zipInstaller;

        public InstallService(Logger logger, ZipInstaller zipInstaller)
        {
            _logger = logger;
            _zipInstaller = zipInstaller;
        }

        public void Install(string installerLocation, FlightPlan flightPlan, InstallOptions installOptions)
        {
            _logger.Info("Begignig installation of " + flightPlan.Id);
            _zipInstaller.Install(installerLocation, flightPlan, installOptions);
        }
    }


    public class ZipInstaller
    {
        private readonly Logger _logger;
        private readonly IDownloadService _downloadService;
        private readonly IPathResolver _pathResolver;

        public ZipInstaller(Logger logger, CompressionService compressionService, IDownloadService downloadService, IPathResolver pathResolver)
        {
            _logger = logger;
            _downloadService = downloadService;
            _pathResolver = pathResolver;
        }

        public void Install(string installerLocation, FlightPlan flightPlan, InstallOptions installOptions)
        {

        }
    }
}
