using AppGet.Commands.Install;
using AppGet.Compression;
using AppGet.Download;
using AppGet.FlightPlans;
using AppGet.HostSystem;
using NLog;

namespace AppGet.Install
{
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