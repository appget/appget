using System.Linq;
using System.Threading.Tasks;
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
        private readonly Logger _logger;

        public UninstallCommandHandler(IPackageRepository packageRepository,
                                       IPackageManifestService packageManifestService,
                                       IWindowsInstallerInventoryManager windowsInstallerInventoryManager,
                                       IUninstallService uninstallService,
                                       Logger logger)
        {
            _packageRepository = packageRepository;
            _packageManifestService = packageManifestService;
            _windowsInstallerInventoryManager = windowsInstallerInventoryManager;
            _uninstallService = uninstallService;
            _logger = logger;
        }

        public bool CanExecute(AppGetOption commandOptions)
        {
            return commandOptions is UninstallOptions;
        }

        public async Task Execute(AppGetOption commandOptions)
        {
            var uninstallOptions = (UninstallOptions)commandOptions;

            var windowsInventory = _windowsInstallerInventoryManager.GetInstalledApplications().Where(c => c.Name.Contains(uninstallOptions.Package)).ToList();
            _logger.Warn($"Package {uninstallOptions.PackageId} wasn't installed using AppGet. Searching Windows installer records");

            // TODO: Uninstall

        }
    }
}
