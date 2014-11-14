using System;
using AppGet.FileTransfer;
using AppGet.HostSystem;
using AppGet.InstalledPackages;
using AppGet.Installers;
using AppGet.Manifests;
using AppGet.Options;
using AppGet.PackageRepository;
using NLog;

namespace AppGet.Commands.Install
{
    public class InstallCommandHandler : ICommandHandler
    {
        private readonly IPackageRepository _packageRepository;
        private readonly IPathResolver _pathResolver;
        private readonly IFileTransferService _fileTransferService;
        private readonly IPackageManifestService _packageManifestService;
        private readonly IInstallService _installService;
        private readonly IFindInstaller _findInstaller;
        private readonly IInventoryManager _inventoryManager;
        private readonly Logger _logger;

        public InstallCommandHandler(IPackageRepository packageRepository, IPathResolver pathResolver, IFileTransferService fileTransferService,
            IPackageManifestService packageManifestService, IInstallService installService, IFindInstaller findInstaller, IInventoryManager inventoryManager, Logger logger)
        {
            _packageRepository = packageRepository;
            _pathResolver = pathResolver;
            _fileTransferService = fileTransferService;
            _packageManifestService = packageManifestService;
            _installService = installService;
            _findInstaller = findInstaller;
            _inventoryManager = inventoryManager;
            _logger = logger;
        }

        public bool CanExecute(AppGetOption packageCommandOptions)
        {
            return packageCommandOptions is InstallOptions;
        }

        public void Execute(AppGetOption searchCommandOptions)
        {

            var installOptions = (InstallOptions)searchCommandOptions;

            if (_inventoryManager.IsInstalled(installOptions.PackageId))
            {
                throw new PackageAlreadyInstalledException(installOptions.PackageId);
            }


            var package = _packageRepository.GetLatest(installOptions.PackageId);
            if (package == null)
            {
                throw new PackageNotFoundException(installOptions.PackageId);
            }

            var manifest = _packageManifestService.LoadManifest(package);

            var installer = _findInstaller.GetBestInstaller(manifest.Installers);


            var installerPath = _fileTransferService.TransferFile(installer.Location, _pathResolver.TempFolder);

            _installService.Install(installerPath, manifest, installOptions);

            _inventoryManager.AddInstalledPackage(package);

        }
    }
}
