using System.Linq;
using AppGet.InstalledPackages;
using NLog;

namespace AppGet.Commands.List
{
    public class ListCommandHandler : ICommandHandler
    {
        private readonly IInventoryManager _inventoryManager;
        private readonly Logger _logger;

        public ListCommandHandler(IInventoryManager inventoryManager, Logger logger)
        {
            _inventoryManager = inventoryManager;
            _logger = logger;
        }

        public bool CanExecute(AppGetOption commandOptions)
        {
            return commandOptions is ListOptions;
        }

        public void Execute(AppGetOption commandOptions)
        {
            var packages = _inventoryManager.GetInstalledPackages();

            if (packages.Any())
            {
                _logger.Info("Installed packages:");
                foreach (var packageInfo in packages)
                {
                    _logger.Info(packageInfo);
                }
            }
            else
            {
                _logger.Info("You have no packages installed.");
            }

        }
    }
}
