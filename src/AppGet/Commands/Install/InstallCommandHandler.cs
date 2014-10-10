using System;
using System.Linq;
using AppGet.Download;
using AppGet.HostSystem;
using AppGet.Install;
using AppGet.Options;
using AppGet.PackageProvider;
using NLog;

namespace AppGet.Commands.Install
{
    public class InstallCommandHandler : ICommandHandler
    {
        private readonly IPackageProvider _packageProvider;
        private readonly IPathResolver _pathResolver;
        private readonly IDownloadService _downloadService;
        private readonly IInstallService _installService;
        private readonly Logger _logger;

        public InstallCommandHandler(IPackageProvider packageProvider, IPathResolver pathResolver, IDownloadService downloadService, IInstallService installService, Logger logger)
        {
            _packageProvider = packageProvider;
            _pathResolver = pathResolver;
            _downloadService = downloadService;
            _installService = installService;
            _logger = logger;
        }

        public bool CanExecute(CommandOptions commandOptions)
        {
            return commandOptions is InstallOptions;
        }

        public void Execute(CommandOptions commandOptions)
        {

            var installOptions = (InstallOptions)commandOptions;
   
            var flightPlan = _packageProvider.GetFlightPlan(commandOptions.PackageName);

            var package = flightPlan.Packages.Single();

            var installerTempLocation = _pathResolver.GetInstallerDownloadPath(package.FileName);

            _downloadService.DownloadFile(package.Source, installerTempLocation);

            _installService.Install(installerTempLocation, flightPlan, installOptions);

        }
    }
}
