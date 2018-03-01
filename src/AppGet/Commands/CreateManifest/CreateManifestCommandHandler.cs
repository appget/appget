using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
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
        private readonly IManifestBuilder _manifestBuilder;
        private readonly IUrlPrompt _urlPrompt;
        private readonly BooleanPrompt _booleanPrompt;

        public CreateManifestCommandHandler(IBuildInstaller installerBuilder, IPackageManifestService packageManifestService,
           IManifestBuilder manifestBuilder, IUrlPrompt urlPrompt, BooleanPrompt booleanPrompt)
        {
            _installerBuilder = installerBuilder;
            _packageManifestService = packageManifestService;
            _manifestBuilder = manifestBuilder;
            _urlPrompt = urlPrompt;
            _booleanPrompt = booleanPrompt;
        }

        public bool CanExecute(AppGetOption commandOptions)
        {
            return commandOptions is CreateManifestOptions;
        }

        public async Task Execute(AppGetOption appGetOption)
        {
            var createOptions = (CreateManifestOptions)appGetOption;

            var manifest = new PackageManifest { Installers = new List<Installer>() };
            var installer = await _installerBuilder.Populate(createOptions.DownloadUrl, true);
            manifest.Installers.Add(installer);

            _manifestBuilder.Populate(manifest, true);

            while (_booleanPrompt.Request("Add an additional installer for different architecture or version of Windows?", false, true))
            {
                var url = _urlPrompt.Request("Download URL (leave blank to cancel)", "", true);
                if (string.IsNullOrWhiteSpace(url))
                {
                    break;
                }
                manifest.Installers.Add(await _installerBuilder.Populate(url, true));
            }


            _packageManifestService.PrintManifest(manifest);


            _packageManifestService.WriteManifest(manifest, "C:\\git\\AppGet.Packages\\manifests");
        }
    }
}