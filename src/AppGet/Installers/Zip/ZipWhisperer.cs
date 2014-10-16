using AppGet.Commands.Install;
using AppGet.Commands.Uninstall;
using AppGet.Compression;
using AppGet.FlightPlans;
using AppGet.HostSystem;
using NLog;

namespace AppGet.Installers.Zip
{
    public class ZipWhisperer : IInstallerWhisperer
    {
        private readonly Logger _logger;
        private readonly ICompressionService _compressionService;
        private readonly IPathResolver _pathResolver;

        public ZipWhisperer(Logger logger, ICompressionService compressionService, IPathResolver pathResolver)
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

        public void Unnstall(FlightPlan flightPlan, UninstallOptions installOptions)
        {
            throw new System.NotImplementedException();
        }

        public bool CanHandle(InstallMethodType installMethod)
        {
            return installMethod == InstallMethodType.Zip;
        }
    }
}