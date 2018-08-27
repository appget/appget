using System.Threading.Tasks;
using AppGet.Commands.CreateManifest;
using AppGet.Infrastructure.Composition;
using AppGet.Installers;
using AppGet.Manifests;
using AppGet.PackageRepository;

namespace AppGet.Commands.Install
{
    [Handles(typeof(InstallOptions))]
    public class InstallCommandHandler : ICommandHandler
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


        public async Task Execute(AppGetOption commandOptions)
        {
            var installOptions = (InstallOptions)commandOptions;

            var package = await _packageRepository.GetAsync(installOptions.PackageId, installOptions.Tag);
            var manifest = await _packageManifestService.LoadManifest(package.ManifestPath);

            await _installService.Install(manifest, installOptions);
        }
    }
}