using System.Collections.Generic;
using System.Linq;
using NLog;

namespace AppGet.InstalledPackages
{
    public interface IInstallTracker
    {
        void TakeSnapshot();
        string GetInstalledProductId();
    }

    public class InstallTracker : IInstallTracker
    {
        private readonly IWindowsInstallerInventoryManager _windowsInstallerInventoryManager;
        private readonly Logger _logger;

        private List<UninstallRecord> _snapshottedUninstallRecords; 

        public InstallTracker(IWindowsInstallerInventoryManager windowsInstallerInventoryManager, Logger logger)
        {
            _windowsInstallerInventoryManager = windowsInstallerInventoryManager;
            _logger = logger;
        }

        public void TakeSnapshot()
        {
            _snapshottedUninstallRecords = _windowsInstallerInventoryManager.GetInstalledApplications();
        }

        public string GetInstalledProductId()
        {
            var productIds = GetInstalledProductIds();

            if (productIds.Count == 0)
            {
                _logger.Debug("No new installation records found");
                return null;
            }

            if (productIds.Count > 0)
            {
                _logger.Debug("More than one installation was detected, automatic removal is not possible");
                return null;
            }

            return productIds.Single();
        }

        private List<string> GetInstalledProductIds()
        {
            if (_snapshottedUninstallRecords == null)
            {
                //Should this throw instead?
                return new List<string>(0);
            }

            var records = _windowsInstallerInventoryManager.GetInstalledApplications();

            return records.Except(_snapshottedUninstallRecords).Select(r => r.Id).ToList();
        }
    }
}
