using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppGet.Commands.Install;
using AppGet.FileTransfer;
using AppGet.HostSystem;
using AppGet.InstalledPackages;
using AppGet.Manifests;
using NLog;

namespace AppGet.Installers
{
    public interface IInstallService
    {
        Task Install(PackageManifest packageManifest, InstallOptions installOptions);
    }

    public class InstallService : IInstallService
    {
        private readonly Logger _logger;
        private readonly IInstallTracker _installTracker;
        private readonly IFindInstaller _findInstaller;
        private readonly IPathResolver _pathResolver;
        private readonly IFileTransferService _fileTransferService;
        private readonly IInventoryManager _inventoryManager;
        private readonly List<IInstallerWhisperer> _installWhisperers;

        public InstallService(Logger logger, IInstallTracker installTracker, IFindInstaller findInstaller, IPathResolver pathResolver,
            IFileTransferService fileTransferService, IInventoryManager inventoryManager, List<IInstallerWhisperer> installWhisperers)
        {
            _logger = logger;
            _installTracker = installTracker;
            _findInstaller = findInstaller;
            _pathResolver = pathResolver;
            _fileTransferService = fileTransferService;
            _inventoryManager = inventoryManager;
            _installWhisperers = installWhisperers;
        }

        public async Task Install(PackageManifest packageManifest, InstallOptions installOptions)
        {
            _logger.Info("Beginning installation of [{0}] {1}", packageManifest.Id, packageManifest.Name);

            var whisperer = _installWhisperers.Single(c => c.CanHandle(packageManifest.InstallMethod));

            var installer = _findInstaller.GetBestInstaller(packageManifest.Installers);
            var installerPath = await _fileTransferService.TransferFile(installer.Location, _pathResolver.TempFolder, installer.GetFileHash());
            _installTracker.TakeSnapshot();

            whisperer.Install(installerPath, packageManifest, installOptions);

            var installedPackage = GetInstalledPackage(packageManifest, installer);

            _inventoryManager.AddPackage(installedPackage);

            _logger.Info("Installation completed for [{0}] {1}", packageManifest.Id, packageManifest.Name);
        }


        private InstalledPackage GetInstalledPackage(PackageManifest manifest, Installer installer)
        {
            var installedPackage = new InstalledPackage
            {
                Id = manifest.Id,
                Name = manifest.Name,
                Version = manifest.Version,
                InstallMethod = manifest.InstallMethod,
                Architecture = installer.Architecture
            };

            if (manifest.InstallMethod == InstallMethodTypes.Zip)
            {
                return installedPackage;
            }

            if (installer.ProductIds != null && installer.ProductIds.Any())
            {
                installedPackage.ProductIds = installer.ProductIds;

                return installedPackage;
            }

            var productId = _installTracker.GetInstalledProductId();

            if (productId == null)
            {
                return installedPackage;
            }

            installedPackage.ProductIds.Add(productId);

            return installedPackage;
        }
    }
}
