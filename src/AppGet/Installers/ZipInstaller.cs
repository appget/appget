using AppGet.Commands.Install;
using AppGet.Compression;
using AppGet.FlightPlans;
using AppGet.HostSystem;
using NLog;

namespace AppGet.Installers
{
    public class ZipInstaller
    {
        private readonly Logger _logger;
        private readonly ICompressionService _compressionService;
        private readonly IPathResolver _pathResolver;

        public ZipInstaller(Logger logger, ICompressionService compressionService, IPathResolver pathResolver)
        {
            _logger = logger;
            _compressionService = compressionService;
            _pathResolver = pathResolver;
        }

        public void Install(string installerLocation, FlightPlan flightPlan, InstallOptions installOptions)
        {
            var target = _pathResolver.GetInstallationPath(flightPlan);
            _compressionService.Decompress(installerLocation, target);
        }
    }
}