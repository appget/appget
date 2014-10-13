using AppGet.FileTransfer;
using AppGet.FlightPlans;
using AppGet.HostSystem;
using AppGet.InstalledPackages;
using AppGet.Installers;
using AppGet.Options;
using AppGet.PackageRepository;
using AppGet.Packages;
using NLog;

namespace AppGet.Commands.Install
{
    public class InstallCommandHandler : ICommandHandler
    {
        private readonly IPackageRepository _packageRepository;
        private readonly IPathResolver _pathResolver;
        private readonly IFileTransferService _fileTransferService;
        private readonly IFlightPlanService _flightPlanService;
        private readonly IInstallService _installService;
        private readonly IFindInstaller _findInstaller;
        private readonly IInventoryManager _inventoryManager;
        private readonly Logger _logger;

        public InstallCommandHandler(IPackageRepository packageRepository, IPathResolver pathResolver, IFileTransferService fileTransferService,
            IFlightPlanService flightPlanService, IInstallService installService, IFindInstaller findInstaller, IInventoryManager inventoryManager, Logger logger)
        {
            _packageRepository = packageRepository;
            _pathResolver = pathResolver;
            _fileTransferService = fileTransferService;
            _flightPlanService = flightPlanService;
            _installService = installService;
            _findInstaller = findInstaller;
            _inventoryManager = inventoryManager;
            _logger = logger;
        }

        public bool CanExecute(CommandOptions commandOptions)
        {
            return commandOptions is InstallOptions;
        }

        public void Execute(CommandOptions commandOptions)
        {

            var installOptions = (InstallOptions)commandOptions;

            var package = _packageRepository.FindPackage(commandOptions.PackageName);
            if (package == null)
            {
                throw new PackageNotFoundException(installOptions.PackageName);
            }

            var flightPlan = _flightPlanService.LoadFlightPlan(package);

            var installer = _findInstaller.GetPackage(flightPlan.Installers);

            var installerTempLocation = _pathResolver.GetInstallerTempPath(installer.FileName);

            _fileTransferService.TransferFile(installer.Location, installerTempLocation);

            _installService.Install(installerTempLocation, flightPlan, installOptions);

            _inventoryManager.AddInstalledPackage(package);

        }
    }
}
