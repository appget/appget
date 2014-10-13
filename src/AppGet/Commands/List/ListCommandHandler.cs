using System;
using System.Linq;
using AppGet.InstalledPackages;
using AppGet.Options;
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

        public bool CanExecute(CommandOptions commandOptions)
        {
            return commandOptions is ListOptions;
        }

        public void Execute(CommandOptions commandOptions)
        {
            var packages = _inventoryManager.GetInstalledPackages();

            if (packages.Any())
            {
                foreach (var packageInfo in packages)
                {
                    _logger.Info(packageInfo);
                }
                
                Console.WriteLine();
                _logger.Info("{0} package(s) are installed.", packages.Count);
            }
            else
            {
                _logger.Info("You have no packages installed.");
            }

        }
    }
}
