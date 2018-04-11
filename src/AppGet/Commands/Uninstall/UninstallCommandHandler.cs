using System.Linq;
using AppGet.InstalledPackages;
using NLog;

namespace AppGet.Commands.Uninstall
{
    public class UninstallCommandHandler : ICommandHandler
    {
        private readonly IWindowsInstallerInventoryManager _windowsInstallerInventoryManager;

        private readonly Logger _logger;

        public UninstallCommandHandler(IWindowsInstallerInventoryManager windowsInstallerInventoryManager,
                                       Logger logger)
        {
            _windowsInstallerInventoryManager = windowsInstallerInventoryManager;
            _logger = logger;
        }

        public bool CanExecute(AppGetOption commandOptions)
        {
            return commandOptions is UninstallOptions;
        }

        public  void Execute(AppGetOption commandOptions)
        {
            var uninstallOptions = (UninstallOptions)commandOptions;

            var windowsInventory = _windowsInstallerInventoryManager.GetInstalledApplications().Where(c => c.Name.Contains(uninstallOptions.Package)).ToList();
            _logger.Warn($"Package {uninstallOptions.PackageId} wasn't installed using AppGet. Searching Windows installer records");

            // TODO: Uninstall

        }
    }
}
