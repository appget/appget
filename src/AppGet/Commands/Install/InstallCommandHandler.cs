using System;
using System.Linq;
using AppGet.Download;
using AppGet.FlightPlans;
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
        private readonly IFlightPlanService _flightPlanService;
        private readonly IInstallService _installService;
        private readonly Logger _logger;

        public InstallCommandHandler(IPackageProvider packageProvider, IPathResolver pathResolver, IDownloadService downloadService,IFlightPlanService flightPlanService, IInstallService installService, Logger logger)
        {
            _packageProvider = packageProvider;
            _pathResolver = pathResolver;
            _downloadService = downloadService;
            _flightPlanService = flightPlanService;
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

            var package = _packageProvider.FindPackage(commandOptions.PackageName);
            if (package == null)
            {
                throw new PackageNotFoundException(installOptions.PackageName);
            }

            var flightPlan = _flightPlanService.LoadFlightPlan(package);

            var installer = flightPlan.Packages.Single();

            var installerTempLocation = _pathResolver.GetInstallerDownloadPath(installer.FileName);

            _downloadService.DownloadFile(installer.Source, installerTempLocation);

            _installService.Install(installerTempLocation, flightPlan, installOptions);

        }
    }
}
