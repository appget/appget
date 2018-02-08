using System.Collections.Generic;
using AppGet.CreatePackage;
using AppGet.Manifests;

namespace AppGet.Commands.CreateManifest
{
    public class CreateManifestCommandHandler : ICommandHandler
    {
        private readonly IXRayService _xRayService;
        private readonly IPackageManifestService _packageManifestService;
        private readonly IEnumerable<IPopulateManifest> _populaters;

        public CreateManifestCommandHandler(IXRayService xRayService, IPackageManifestService packageManifestService, IEnumerable<IPopulateManifest> populaters)
        {
            _xRayService = xRayService;
            _packageManifestService = packageManifestService;
            _populaters = populaters;
        }

        public bool CanExecute(AppGetOption commandOptions)
        {
            return commandOptions is CreateManifestOptions;
        }

        public void Execute(AppGetOption appGetOption)
        {
            var createOptions = (CreateManifestOptions)appGetOption;

            var manifest = new PackageManifest();
            manifest.Installers = new List<Installer>();


            var installer = _xRayService.Scan(createOptions.DownloadUrl);


            manifest.Installers.Add(installer);

            foreach (var populater in _populaters)
            {
                populater.Populate(manifest);
            }


            _packageManifestService.PrintManifest(manifest);
        }
    }
}