using System.Linq;
using AppGet.FileTransfer;
using AppGet.HostSystem;
using AppGet.InstalledPackages;
using AppGet.Installers;
using AppGet.Manifests;
using AppGet.PackageRepository;
using NLog;

namespace AppGet.Commands.Install
{
    public class InstallCommandHandler : ICommandHandler
    {
        private readonly IPackageRepository _packageRepository;
        private readonly IPackageManifestService _packageManifestService;
        private readonly IInstallService _installService;
        private readonly IInventoryManager _inventoryManager;

        public InstallCommandHandler(IPackageRepository packageRepository,
                                     IPackageManifestService packageManifestService,
                                     IInstallService installService,
                                     IInventoryManager inventoryManager)
        {
            _packageRepository = packageRepository;
            _packageManifestService = packageManifestService;
            _installService = installService;
            _inventoryManager = inventoryManager;
        }

        public bool CanExecute(AppGetOption commandOptions)
        {
            return commandOptions is InstallOptions;
        }

        public void Execute(AppGetOption commandOptions)
        {
            var installOptions = (InstallOptions)commandOptions;

            if (!installOptions.Force && _inventoryManager.IsInstalled(installOptions.PackageId))
            {
                throw new PackageAlreadyInstalledException(installOptions.PackageId);
            }

            var package = _packageRepository.GetLatest(installOptions.PackageId);

            if (package == null)
            {
                throw new PackageNotFoundException(installOptions.PackageId);
            }

            var manifest = _packageManifestService.LoadManifest(package);

            _installService.Install(manifest, installOptions);
        }
    }
}
