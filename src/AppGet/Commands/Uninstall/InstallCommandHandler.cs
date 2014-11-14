using AppGet.InstalledPackages;
using AppGet.Installers;
using AppGet.Manifests;
using AppGet.Options;
using AppGet.PackageRepository;

namespace AppGet.Commands.Uninstall
{
    public class UninstallCommandHandler : ICommandHandler
    {
        private readonly IPackageRepository _packageRepository;
        private readonly IPackageManifestService _packageManifestService;
        private readonly IUninstallService _uninstallService;
        private readonly IInventoryManager _inventoryManager;

        public UninstallCommandHandler(IPackageRepository packageRepository,
            IPackageManifestService packageManifestService, IUninstallService uninstallService, IInventoryManager inventoryManager)
        {
            _packageRepository = packageRepository;
            _packageManifestService = packageManifestService;
            _uninstallService = uninstallService;
            _inventoryManager = inventoryManager;
        }

        public bool CanExecute(AppGetOption packageCommandOptions)
        {
            return packageCommandOptions is UninstallOptions;
        }

        public void Execute(AppGetOption searchCommandOptions)
        {
            var uninstallOptions = (UninstallOptions)searchCommandOptions;

            if (!_inventoryManager.IsInstalled(uninstallOptions.PackageId))
            {
                throw new PackageNotInstalledException(uninstallOptions.PackageId);
            }

            var package = _packageRepository.GetLatest(uninstallOptions.PackageId);
            if (package == null)
            {
                throw new PackageNotFoundException(uninstallOptions.PackageId);
            }


            var manifest = _packageManifestService.LoadManifest(package);

            _uninstallService.Uninstall(manifest, uninstallOptions);

            _inventoryManager.RemoveInstalledPackage(package);
        }
    }
}
