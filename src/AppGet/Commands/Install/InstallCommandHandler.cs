using System.Threading.Tasks;
using AppGet.Installers;
using AppGet.Manifests;
using AppGet.PackageRepository;

namespace AppGet.Commands.Install
{
    public class InstallCommandHandler : ICommandHandler<InstallOptions>
    {
        private readonly IPackageRepository _packageRepository;
        private readonly IPackageManifestService _packageManifestService;
        private readonly IInstallService _installService;

        public InstallCommandHandler(IPackageRepository packageRepository, IPackageManifestService packageManifestService, IInstallService installService)
        {
            _packageRepository = packageRepository;
            _packageManifestService = packageManifestService;
            _installService = installService;
        }


        public async Task Execute(InstallOptions commandOptions)
        {
            var installOptions = (InstallOptions)commandOptions;

            var package = await _packageRepository.GetAsync(installOptions.PackageId, installOptions.Tag);
            var manifest = await _packageManifestService.LoadManifest(package.ManifestPath);

            await _installService.Install(manifest, installOptions);
        }
    }
}