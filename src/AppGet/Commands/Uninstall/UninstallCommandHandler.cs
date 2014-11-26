using System.Linq;
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
                                       IPackageManifestService packageManifestService,
                                       IUninstallService uninstallService,
                                       IInventoryManager inventoryManager)
        {
            _packageRepository = packageRepository;
            _packageManifestService = packageManifestService;
            _uninstallService = uninstallService;
            _inventoryManager = inventoryManager;
        }

        public bool CanExecute(AppGetOption commandOptions)
        {
            return commandOptions is UninstallOptions;
        }

        public void Execute(AppGetOption commandOptions)
        {
            var uninstallOptions = (UninstallOptions)commandOptions;

            var installedPackages = _inventoryManager.GetInstalledPackages(uninstallOptions.PackageId);

            if (!installedPackages.Any())
            {
                throw new PackageNotInstalledException(uninstallOptions.PackageId);
            }

            foreach (var installedPackage in installedPackages)
            {
                var package = _packageRepository.GetLatest(uninstallOptions.PackageId);

                if (package == null)
                {
                    throw new PackageNotFoundException(uninstallOptions.PackageId);
                }

                var manifest = _packageManifestService.LoadManifest(package);

                //TODO: Does the uninstall service know how to choose the correct package to remove?
                _uninstallService.Uninstall(manifest, uninstallOptions);
                _inventoryManager.RemovePackage(installedPackage);
            }
        }
    }
}
