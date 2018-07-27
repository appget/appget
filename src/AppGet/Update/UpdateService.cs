using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppGet.WindowsInstaller;

namespace AppGet.Update
{
    public class UpdateService
    {
        private readonly NovoClient _novoClient;
        private readonly WindowsInstallerClient _windowsInstallerClient;

        public UpdateService(NovoClient novoClient, WindowsInstallerClient windowsInstallerClient)
        {
            _novoClient = novoClient;
            _windowsInstallerClient = windowsInstallerClient;
        }

        public async Task<List<PackageUpdate>> GetUpdates()
        {
            var records = _windowsInstallerClient.GetRecords();

            return await _novoClient.GetUpdates(records);
        }
    }
}
