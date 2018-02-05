using System;
using AppGet.Manifests;
using AppGet.Options;
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

            var package = _packageRepository.GetLatest(viewOptions.PackageId);
            if (package == null)
            {
                throw new PackageNotFoundException(viewOptions.PackageId);
            }

            var manifest = _packageManifestService.ReadManifest(package);
            Console.WriteLine("===============================================");
            Console.WriteLine();
            Console.WriteLine(manifest);
            Console.WriteLine();
            Console.WriteLine("===============================================");


        }
    }
}