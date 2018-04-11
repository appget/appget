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

        public InstallCommandHandler(IPackageRepository packageRepository,
                                     IPackageManifestService packageManifestService,
                                     IInstallService installService)
        {
            _packageRepository = packageRepository;
            _packageManifestService = packageManifestService;
            _installService = installService;
        }

        public bool CanExecute(AppGetOption commandOptions)
        {
            return commandOptions is InstallOptions;
        }

        public  void Execute(AppGetOption commandOptions)
        {
            var installOptions = (InstallOptions)commandOptions;

            var package =  _packageRepository.Get(installOptions.PackageId, installOptions.PackageTag);

            var manifest =  _packageManifestService.LoadManifest(package.ManifestPath);

             _installService.Install(manifest, installOptions);
        }
    }
}
