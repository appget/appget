using System.Collections.Generic;
using System.Diagnostics;
using AppGet.CommandLine.Prompts;
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
        private readonly IUrlPrompt _urlPrompt;
        private readonly BooleanPrompt _booleanPrompt;

        public CreateManifestCommandHandler(IBuildInstaller installerBuilder, IPackageManifestService packageManifestService,
            IEnumerable<IPopulateManifest> populaters, IUrlPrompt urlPrompt, BooleanPrompt booleanPrompt)
        {
            _installerBuilder = installerBuilder;
            _packageManifestService = packageManifestService;
            _populaters = populaters;
            _urlPrompt = urlPrompt;
            _booleanPrompt = booleanPrompt;
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
            manifest.Installers.Add(installer);

            var fileVersionInfo = FileVersionInfo.GetVersionInfo(installer.FilePath);



            foreach (var populater in _populaters)
            {
                populater.Populate(manifest, fileVersionInfo);
            }

            while (_booleanPrompt.Request("Add an additional installer for different architecture or version of Windows?", false))
            {
                var url = _urlPrompt.Request("Download URL (leave blank to cancel)", "");
                if (string.IsNullOrWhiteSpace(url))
                {
                    break;
                }
                manifest.Installers.Add(_installerBuilder.Populate(url));
            }


            _packageManifestService.PrintManifest(manifest);


            _packageManifestService.WriteManifest(manifest, "C:\\git\\AppGet.Packages\\manifests");
        }
    }
}