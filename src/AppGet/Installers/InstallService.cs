using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppGet.Commands.Install;
using AppGet.FileTransfer;
using AppGet.HostSystem;
using AppGet.Manifest;
using AppGet.Manifests;
using AppGet.Windows;
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
        private readonly IFindInstaller _findInstaller;
        private readonly IPathResolver _pathResolver;
        private readonly IFileTransferService _fileTransferService;
        private readonly List<IInstallerWhisperer> _installWhisperers;
        private readonly IUnlocker _unlocker;

        public InstallService(Logger logger, IFindInstaller findInstaller, IPathResolver pathResolver,
            IFileTransferService fileTransferService, List<IInstallerWhisperer> installWhisperers, IUnlocker unlocker)
        {
            _logger = logger;
            _findInstaller = findInstaller;
            _pathResolver = pathResolver;
            _fileTransferService = fileTransferService;
            _installWhisperers = installWhisperers;
            _unlocker = unlocker;
        }

        public async Task Install(PackageManifest packageManifest, InstallOptions installOptions)
        {
            _logger.Info("Beginning installation of '{0}'", packageManifest);

            var whisperer = _installWhisperers.Single(c => c.CanHandle(packageManifest.InstallMethod));

            var installer = _findInstaller.GetBestInstaller(packageManifest.Installers);
            var installerPath = _fileTransferService.TransferFile(installer.Location, _pathResolver.TempFolder, installer.Sha256);


            await _unlocker.UnlockFolder(packageManifest);

            whisperer.Install(installerPath, packageManifest, installOptions);

            _logger.Info("Installation completed succesfully for '{0}'", packageManifest);
        }
    }
}