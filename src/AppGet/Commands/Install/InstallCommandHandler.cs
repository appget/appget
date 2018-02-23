using System.Threading.Tasks;
using AppGet.InstalledPackages;
using AppGet.Installers;
using AppGet.Manifests;
using AppGet.PackageRepository;

namespace AppGet.Commands.Install
{
    public class InstallCommandHandler : ICommandHandler
    {
        private readonly IPackageRepository _packageRepository;
        private readonly IPackageManifestService _packageManifestService;
        private readonly IInstallService _installService;
        private readonly IInventoryManager _inventoryManager;

        public InstallCommandHandler(IPackageRepository packageRepository,
                                     IPackageManifestService packageManifestService,
                                     IInstallService installService,
                                     IInventoryManager inventoryManager)
        {
            _packageRepository = packageRepository;
            _packageManifestService = packageManifestService;
            _installService = installService;
            _inventoryManager = inventoryManager;
        }

        public bool CanExecute(AppGetOption commandOptions)
        {
            return commandOptions is InstallOptions;
        }

        public async Task Execute(AppGetOption commandOptions)
        {
            var installOptions = (InstallOptions)commandOptions;

            if (!installOptions.Force && _inventoryManager.IsInstalled(installOptions.PackageId))
            {
                throw new PackageAlreadyInstalledException(installOptions.PackageId);
            }

            var package = await _packageRepository.GetLatest(installOptions.PackageId);

            if (package == null)
            {
                throw new PackageNotFoundException(installOptions.PackageId);
            }

            var manifest = await _packageManifestService.LoadManifest(package.ManifestUrl);

            await _installService.Install(manifest, installOptions);
        }
    }
}
