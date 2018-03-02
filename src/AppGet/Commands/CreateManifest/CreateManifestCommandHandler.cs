using System.Threading.Tasks;
using AppGet.CommandLine.Prompts;
using AppGet.CreatePackage;
using AppGet.CreatePackage.Installer;
using AppGet.Manifests;

namespace AppGet.Commands.CreateManifest
{
    public class CreateManifestCommandHandler : ICommandHandler
    {
        private readonly IComposeInstaller _installerBuilder;
        private readonly IPackageManifestService _packageManifestService;
        private readonly IComposeManifest _composeManifest;
        private readonly IUrlPrompt _urlPrompt;
        private readonly BooleanPrompt _booleanPrompt;

        public CreateManifestCommandHandler(IComposeInstaller installerBuilder, IPackageManifestService packageManifestService,
           IComposeManifest composeManifest, IUrlPrompt urlPrompt, BooleanPrompt booleanPrompt)
        {
            _installerBuilder = installerBuilder;
            _packageManifestService = packageManifestService;
            _composeManifest = composeManifest;
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

            var manifest = new PackageManifestBuilder();
            var installer = await _installerBuilder.Compose(createOptions.DownloadUrl, true);
            manifest.Installers.Add(installer);

            _composeManifest.Compose(manifest, true);

            while (_booleanPrompt.Request("Add an additional installer for different architecture or version of Windows?", false))
            {
                var url = _urlPrompt.Request("Download URL (leave blank to cancel)", "");
                if (string.IsNullOrWhiteSpace(url))
                {
                    break;
                }
                manifest.Installers.Add(await _installerBuilder.Compose(url, true));
            }


            _packageManifestService.PrintManifest(manifest.Build());


            _packageManifestService.WriteManifest(manifest, "C:\\git\\AppGet.Packages\\manifests");
        }
    }
}