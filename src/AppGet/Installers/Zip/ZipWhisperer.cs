using System.IO;
using AppGet.Commands.Install;
using AppGet.Commands.Uninstall;
using AppGet.Compression;
using AppGet.FileSystem;
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
        private readonly IFileSystem _fileSystem;

        public ZipWhisperer(Logger logger, ICompressionService compressionService, IPathResolver pathResolver, IFileSystem fileSystem)
        {
            _logger = logger;
            _compressionService = compressionService;
            _pathResolver = pathResolver;
            _fileSystem = fileSystem;
        }

        public void Install(string installerLocation, FlightPlan flightPlan, InstallOptions installOptions)
        {
            var target = _pathResolver.GetInstallationPath(flightPlan);
            _compressionService.Decompress(installerLocation, target);
        }

        public void Uninstall(FlightPlan flightPlan, UninstallOptions installOptions)
        {
            var target = _pathResolver.GetInstallationPath(flightPlan);
            _logger.Info("Deleting {0}", target);

            try
            {
                _fileSystem.DeleteDirectory(target);
            }
            catch (DirectoryNotFoundException e)
            {
                _logger.Warn(e.Message);
            }
        }

        public bool CanHandle(InstallMethodType installMethod)
        {
            return installMethod == InstallMethodType.Zip;
        }
    }
}