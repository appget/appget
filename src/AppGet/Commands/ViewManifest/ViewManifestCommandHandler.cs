using AppGet.Manifests;
using AppGet.PackageRepository;

namespace AppGet.Commands.ViewManifest
{
    public class ViewManifestCommandHandler : ICommandHandler
    {
        private readonly IPackageRepository _packageRepository;
        private readonly IPackageManifestService _packageManifestService;

        public ViewManifestCommandHandler(IPackageRepository packageRepository, IPackageManifestService packageManifestService)
        {
            _packageRepository = packageRepository;
            _packageManifestService = packageManifestService;
        }

        public bool CanExecute(AppGetOption commandOptions)
        {
            return commandOptions is ViewManifestOptions;
        }

        public void Execute(AppGetOption searchCommandOptions)
        {
            var viewOptions = (ViewManifestOptions)searchCommandOptions;
            var package = _packageRepository.Get(viewOptions.PackageId, viewOptions.Tag);
            var manifest = _packageManifestService.LoadManifest(package.ManifestPath);
            _packageManifestService.PrintManifest(manifest);
        }
    }
}