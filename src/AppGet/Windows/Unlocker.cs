using System;
using System.Linq;
using AppGet.Manifest;
using JetBrains.Annotations;
using NLog;

namespace AppGet.Windows
{
    public interface IUnlocker
    {
        void UnlockFolder(string installationPath, InstallMethodTypes installMethod);
    }

    [UsedImplicitly]
    public class Unlocker : IUnlocker
    {
        private readonly IProcessController _processController;
        private readonly ManagementObject _managementObject;
        private readonly Logger _logger;

        public Unlocker(IProcessController processController, ManagementObject managementObject, Logger logger)
        {
            _processController = processController;
            _managementObject = managementObject;
            _logger = logger;
        }

        public void UnlockFolder(string installationPath, InstallMethodTypes installMethod)
        {
            if (installationPath == null || installMethod == InstallMethodTypes.MSI || installMethod == InstallMethodTypes.Squirrel) return;

            _logger.Trace("Getting list of processes that are locking {0}", installationPath);
            var processIds = _managementObject.GetProcessByPath(installationPath).ToList();
            _logger.Trace("Found {0} processes", processIds.Count);

            foreach (var processId in processIds)
            {
                var process = _processController.TryGetRunningProcess(processId);

                if (process == null || process.ProcessName == "System" || process.Id == _processController.GetCurrentProcess().Id) continue;

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