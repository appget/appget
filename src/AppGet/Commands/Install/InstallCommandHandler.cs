using System.Linq;
using AppGet.FileTransfer;
using AppGet.HostSystem;
using AppGet.InstalledPackages;
using AppGet.Installers;
using AppGet.Manifests;
using AppGet.Options;
using AppGet.PackageRepository;
using NLog;

namespace AppGet.Commands.Install
{
    public class InstallCommandHandler : ICommandHandler
    {
        private readonly IPackageRepository _packageRepository;
        private readonly IPathResolver _pathResolver;
        private readonly IFileTransferService _fileTransferService;
        private readonly IPackageManifestService _packageManifestService;
        private readonly IInstallService _installService;
        private readonly IFindInstaller _findInstaller;
        private readonly IInventoryManager _inventoryManager;
        private readonly IInstallTracker _installTracker;
        private readonly Logger _logger;

        public InstallCommandHandler(IPackageRepository packageRepository,
                                     IPathResolver pathResolver,
			                         IPackageManifestService packageManifestService,
                                     IFileTransferService fileTransferService,
                                     IInstallService installService,
                                     IFindInstaller findInstaller,
                                     IInventoryManager inventoryManager,
                                     IInstallTracker installTracker,
                                     Logger logger)
        {
            _packageRepository = packageRepository;
            _pathResolver = pathResolver;
            _fileTransferService = fileTransferService;
            _packageManifestService = packageManifestService;
            _installService = installService;
            _findInstaller = findInstaller;
            _inventoryManager = inventoryManager;
            _installTracker = installTracker;
            _logger = logger;
        }

        public bool CanExecute(AppGetOption commandOptions)
        {
            return commandOptions is InstallOptions;
        }

        public void Execute(AppGetOption commandOptions)
        {
            var installOptions = (InstallOptions)commandOptions;

            if (_inventoryManager.IsInstalled(installOptions.PackageId))
            {
                throw new PackageAlreadyInstalledException(installOptions.PackageId);
            }

            var package = _packageRepository.GetLatest(installOptions.PackageId);

            if (package == null)
            {
                throw new PackageNotFoundException(installOptions.PackageId);
            }

            var manifest = _packageManifestService.LoadManifest(package);
            var installer = _findInstaller.GetBestInstaller(manifest.Installers);
            var installerPath = _fileTransferService.TransferFile(installer.Location, _pathResolver.TempFolder);
            
            _installTracker.TakeSnapshot();
            _installService.Install(installerPath, manifest, installOptions);

            var installedPackage = GetInstalledPackage(manifest, installer);

            _inventoryManager.AddPackage(installedPackage);
        }

        private InstalledPackage GetInstalledPackage(PackageManifest flightPlan, Installer installer)
        {
            var installedPackage = new InstalledPackage
                                   {
                                       Id = flightPlan.Id,
                                       Name = flightPlan.Name,
                                       Version = flightPlan.Version,
                                       InstallMethod = flightPlan.InstallMethod,
                                       Architecture = installer.Architecture
                                   };

            if (flightPlan.InstallMethod == InstallMethodType.Zip)
            {
                return installedPackage;
            }

            if (installer.ProductIds.Any())
            {
                installedPackage.ProductIds = installer.ProductIds;

                return installedPackage;
            }

            var productId = _installTracker.GetInstalledProductId();

            if (productId == null)
            {
                return installedPackage;
            }

            installedPackage.ProductIds.Add(productId);

            return installedPackage;
        }
    }
}
