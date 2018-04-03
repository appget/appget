using System.Threading.Tasks;
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

        public async Task Execute(AppGetOption searchCommandOptions)
        {
            var viewOptions = (ViewManifestOptions)searchCommandOptions;
            var package = await _packageRepository.Get(viewOptions.PackageId, viewOptions.PackageTag);
            var manifest = await _packageManifestService.LoadManifest(package.ManifestPath);
            _packageManifestService.PrintManifest(manifest);
        }
    }
}