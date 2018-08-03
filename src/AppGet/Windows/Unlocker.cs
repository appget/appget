using System;
using System.Linq;
using System.Threading.Tasks;
using AppGet.Manifest;
using AppGet.Update;
using NLog;

namespace AppGet.Windows
{
    public interface IUnlocker
    {
        Task UnlockFolder(PackageManifest path);
    }

    public class Unlocker : IUnlocker
    {
        private readonly IProcessController _processController;
        private readonly ManagementObject _managementObject;
        private readonly UpdateService _updateService;
        private readonly Logger _logger;

        public Unlocker(IProcessController processController, ManagementObject managementObject, UpdateService updateService, Logger logger)
        {
            _processController = processController;
            _managementObject = managementObject;
            _updateService = updateService;
            _logger = logger;
        }

        public async Task UnlockFolder(PackageManifest packageManifest)
        {
            if (packageManifest.InstallMethod == InstallMethodTypes.MSI || packageManifest.InstallMethod == InstallMethodTypes.Squirrel) return;

            var update = await _updateService.GetUpdate(packageManifest.Id);
            if (update?.InstallationPath != null)
            {
                _logger.Trace("Getting list of processes that are locking {0}", update.InstallationPath);
                var processIds = _managementObject.GetProcessByPath(update.InstallationPath).ToList();
                _logger.Trace("Found {0} processes", processIds.Count);

                foreach (var processId in processIds)
                {
                    var process = _processController.TryGetRunningProcess(processId);

                    if (process == null || process.ProcessName == "System") continue;

                    try
                    {
                        _processController.Kill(process, 5000);
                    }
                    catch (Exception e)
                    {
                        _logger.Warn(e, "Unable to terminate '{0}'. You might need to terminate the process manually for the installation to succeed.", process.ProcessName);
                    }
                }
            }
        }
    }
}