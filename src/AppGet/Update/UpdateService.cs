using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppGet.HostSystem;
using AppGet.Installers;
using AppGet.Manifest;
using AppGet.PackageRepository;
using AppGet.Windows.WindowsInstaller;
using NLog;
using Console = Colorful.Console;

namespace AppGet.Update
{
    public class UpdateService
    {
        private readonly NovoClient _novoClient;
        private readonly WindowsInstallerClient _windowsInstallerClient;
        private readonly IPackageRepository _packageRepository;
        private readonly IInstallService _installService;
        private readonly IEnvInfo _envInfo;
        private readonly Logger _logger;

        public UpdateService(NovoClient novoClient, WindowsInstallerClient windowsInstallerClient, IPackageRepository packageRepository,
            IInstallService installService, IEnvInfo envInfo, Logger logger)
        {
            _novoClient = novoClient;
            _windowsInstallerClient = windowsInstallerClient;
            _packageRepository = packageRepository;
            _installService = installService;
            _envInfo = envInfo;
            _logger = logger;
        }

        public async Task<List<PackageUpdate>> GetUpdates()
        {
            _logger.Debug("Getting list of installed application");
            var records = _windowsInstallerClient.GetRecords();

            _logger.Info("Getting list of available updates...");
            var updates = await _novoClient.GetUpdates(records);

            Console.WriteLine();
            _logger.Info("Total Applications: {0:n0}   Updates Available: {1:n0}", updates.Count, updates.Count(c => c.Status == UpdateStatus.Available));
            Console.WriteLine();

            return updates;
        }


        public async Task UpdatePackage(string packageId, string tag, string repository, InstallInteractivityLevel interactivityLevel)
        {
            var manifest = await _packageRepository.GetAsync(packageId, tag, repository);
            await _installService.Install(manifest, interactivityLevel);
        }

        public async Task UpdateAllPackages(InstallInteractivityLevel interactivityLevel)
        {
            if (!_envInfo.IsAdministrator)
            {
                Console.WriteLine();
                _logger.Warn("Running as administrator is recommended to allow uninterrupted batch updates.");
                Console.WriteLine();
            }

            var updates = await GetUpdates();
            var toInstall = updates.Where(c => c.Status == UpdateStatus.Available).ToList();

            var updated = 0;
            var failed = 0;
            var restartRequired = false;

            for (var index = 0; index < toInstall.Count; index++)
            {
                var update = toInstall[index];

                try
                {
                    _logger.Info("Installing update {0} of {1}", index + 1, toInstall.Count);
                    Console.WriteLine();
                    await UpdatePackage(update.PackageId, PackageManifest.LATEST_TAG, null, interactivityLevel);
                    updated++;
                }
                catch (InstallerException e) when (e.ExitReason.Category == ExitCodeTypes.RestartRequired)
                {
                    restartRequired = true;
                }
                catch (Exception e)
                {
                    _logger.Fatal(e, "An error occurred while updating {0}", update.PackageId);
                    failed++;
                }
            }

            Console.WriteLine();
            _logger.Info("Updates Applied Successfully: {0:n0}   Updates failed to apply: {1:n0}", updated, failed);

            if (restartRequired)
            {
                _logger.Warn("One or more installers have requested a system restart to complete the installation.");
            }
        }
    }
}
