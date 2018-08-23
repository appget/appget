using System.Threading.Tasks;
using AppGet.Manifests;
using AppGet.PackageRepository;

namespace AppGet.Commands.ViewManifest
{
    public class ViewManifestCommandHandler : ICommandHandler<ViewManifestOptions>
    {
        private readonly IPackageRepository _packageRepository;
        private readonly IPackageManifestService _packageManifestService;

        public ViewManifestCommandHandler(IPackageRepository packageRepository, IPackageManifestService packageManifestService)
        {
            _packageRepository = packageRepository;
            _packageManifestService = packageManifestService;
        }

        public async Task Execute(ViewManifestOptions searchCommandOptions)
        {
            var viewOptions = (ViewManifestOptions)searchCommandOptions;
            var package = await _packageRepository.GetAsync(viewOptions.PackageId, viewOptions.Tag);
            var manifest = await _packageManifestService.LoadManifest(package.ManifestPath);
            _packageManifestService.PrintManifest(manifest);
        }
    }
}