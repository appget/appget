using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppGet.WindowsInstaller;
using NLog;

namespace AppGet.Update
{
    public class UpdateService
    {
        private readonly NovoClient _novoClient;
        private readonly WindowsInstallerClient _windowsInstallerClient;
        private readonly Logger _logger;

        public UpdateService(NovoClient novoClient, WindowsInstallerClient windowsInstallerClient, Logger logger)
        {
            _novoClient = novoClient;
            _windowsInstallerClient = windowsInstallerClient;
            _logger = logger;
        }

        public async Task<List<PackageUpdate>> GetUpdates()
        {
            _logger.Debug("Getting list of installed application");
            var records = _windowsInstallerClient.GetRecords();

            _logger.Info("Getting list of available updates...");
            return await _novoClient.GetUpdates(records);
        }
    }
}
