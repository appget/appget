using System;
using System.Linq;
using AppGet.InstalledPackages;
using AppGet.Options;
using AppGet.Serialization;

namespace AppGet.Commands.WindowsInstallerSearch
{
    public class WindowsInstallerSearchCommandHandler : ICommandHandler
    {
        private readonly IWindowsInstallerInventoryManager _windowsInstallerInventoryManager;

        public WindowsInstallerSearchCommandHandler(IWindowsInstallerInventoryManager windowsInstallerInventoryManager)
        {
            _windowsInstallerInventoryManager = windowsInstallerInventoryManager;
        }

        public bool CanExecute(AppGetOption commandOptions)
        {
            return commandOptions is WindowsInstallerSearchOptions;
        }

        public void Execute(AppGetOption commandOptions)
        {

            var searchOptions = (WindowsInstallerSearchOptions)commandOptions;

            var packages = _windowsInstallerInventoryManager.GetInstalledApplications();

            var results = packages.Where(c => c.Name.ToLowerInvariant().Contains(searchOptions.PackageId.ToLowerInvariant())).ToList();

            Console.WriteLine();
            Console.WriteLine("Your query matched {0} of {1} installed applications.", results.Count, packages.Count);
            Console.WriteLine();

            foreach (var uninstallRecord in results)
            {
                Console.WriteLine(Yaml.Serialize(uninstallRecord));
            }
        }
    }
}