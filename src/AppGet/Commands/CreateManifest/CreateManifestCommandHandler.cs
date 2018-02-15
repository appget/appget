using System.Collections.Generic;
using System.Diagnostics;
using AppGet.CreatePackage;
using AppGet.CreatePackage.InstallerPopulators;
using AppGet.CreatePackage.ManifestPopulators;
using AppGet.Manifests;

namespace AppGet.Commands.CreateManifest
{
    public class CreateManifestCommandHandler : ICommandHandler
    {
        private readonly IBuildInstaller _installerBuilder;
        private readonly IPackageManifestService _packageManifestService;
        private readonly IEnumerable<IPopulateManifest> _populaters;

        public CreateManifestCommandHandler(IBuildInstaller installerBuilder, IPackageManifestService packageManifestService, IEnumerable<IPopulateManifest> populaters)
        {
            _installerBuilder = installerBuilder;
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

            var manifest = new PackageManifest { Installers = new List<Installer>() };
            var installer = _installerBuilder.Populate(createOptions.DownloadUrl);
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(installer.FilePath);

            manifest.Installers.Add(installer);


            foreach (var populater in _populaters)
            {
                populater.Populate(manifest, fileVersionInfo);
            }

            _packageManifestService.PrintManifest(manifest);


            _packageManifestService.WriteManifest(manifest, "C:\\git\\AppGet.Packages\\manifests");
        }
    }
}