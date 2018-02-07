using System.Linq;
using AppGet.InstalledPackages;
using AppGet.Installers;
using AppGet.Manifests;
using AppGet.PackageRepository;
using NLog;

namespace AppGet.Commands.Uninstall
{
    public class UninstallCommandHandler : ICommandHandler
    {
        private readonly IPackageRepository _packageRepository;
        private readonly IPackageManifestService _packageManifestService;
        private readonly IWindowsInstallerInventoryManager _windowsInstallerInventoryManager;

        private readonly IUninstallService _uninstallService;
        private readonly IInventoryManager _inventoryManager;
        private readonly Logger _logger;

        public UninstallCommandHandler(IPackageRepository packageRepository,
                                       IPackageManifestService packageManifestService,
                                       IWindowsInstallerInventoryManager windowsInstallerInventoryManager,
                                       IUninstallService uninstallService,
                                       IInventoryManager inventoryManager,
                                       Logger logger)
        {
            _packageRepository = packageRepository;
            _packageManifestService = packageManifestService;
            _windowsInstallerInventoryManager = windowsInstallerInventoryManager;
            _uninstallService = uninstallService;
            _inventoryManager = inventoryManager;
            _logger = logger;
        }

        public bool CanExecute(AppGetOption commandOptions)
        {
            return commandOptions is UninstallOptions;
        }

        public void Execute(AppGetOption commandOptions)
        {
            var uninstallOptions = (UninstallOptions)commandOptions;

            var installedPackages = _inventoryManager.GetInstalledPackages(uninstallOptions.PackageId);
            var windowsInventory = _windowsInstallerInventoryManager.GetInstalledApplications(uninstallOptions.PackageId).ToList();

            if (installedPackages.Any())
            {
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
            else
            {
                _logger.Warn($"Package {uninstallOptions.PackageId} wasn't installed using AppGet. Searching Windows installer records");

            }






        }
    }
}
