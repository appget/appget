using System.Threading.Tasks;
using AppGet.Commands.Install;
using AppGet.Installers;
using AppGet.Manifests;
using AppGet.PackageRepository;

namespace AppGet.Commands.Update
{
    public class UpdateCommandHandler : ICommandHandler
    {
        private readonly IPackageRepository _packageRepository;
        private readonly IPackageManifestService _packageManifestService;
        private readonly IInstallService _installService;

        public UpdateCommandHandler(IPackageRepository packageRepository, IPackageManifestService packageManifestService, IInstallService installService)
        {
            _packageRepository = packageRepository;
            _packageManifestService = packageManifestService;
            _installService = installService;
        }

        public bool CanExecute(AppGetOption commandOptions)
        {
            return commandOptions is UpdateOptions;
        }

        public async Task Execute(AppGetOption commandOptions)
        {
            var updateOptions = (UpdateOptions)commandOptions;

            var package = await _packageRepository.GetAsync(updateOptions.PackageId, updateOptions.Tag);
            var manifest = await _packageManifestService.LoadManifest(package.ManifestPath);

            var installOptions = new InstallOptions
            {
                Package = updateOptions.Package,
                Interactive = updateOptions.Interactive,
                Passive = updateOptions.Passive,
                Silent = updateOptions.Silent,
            };

            await _installService.Install(manifest, installOptions);
        }
    }
}