using System;
using System.Linq;
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
        private readonly IXRayClient _xRayClient;
        private readonly BooleanPrompt _booleanPrompt;

        public CreateManifestCommandHandler(IComposeInstaller installerBuilder, IPackageManifestService packageManifestService,
           IComposeManifest composeManifest, IUrlPrompt urlPrompt, IXRayClient xRayClient, BooleanPrompt booleanPrompt)
        {
            _installerBuilder = installerBuilder;
            _packageManifestService = packageManifestService;
            _composeManifest = composeManifest;
            _urlPrompt = urlPrompt;
            _xRayClient = xRayClient;
            _booleanPrompt = booleanPrompt;
        }

        public bool CanExecute(AppGetOption commandOptions)
        {
            return commandOptions is CreateManifestOptions;
        }

        public async Task Execute(AppGetOption appGetOption)
        {
            var createOptions = (CreateManifestOptions)appGetOption;

            if (!Uri.IsWellFormedUriString(createOptions.DownloadUrl, UriKind.Absolute))
            {
                throw new InvalidCommandParamaterException("Invalid URL", createOptions);
            }

            var manifestBuilder = await _xRayClient.GetBuilder(new Uri(createOptions.DownloadUrl));

            await _installerBuilder.Compose(manifestBuilder.Installers.Single(), true);

            _composeManifest.Compose(manifestBuilder, true);

            while (_booleanPrompt.Request("Add an additional installer for different architecture or version of Windows?", false))
            {
                var url = _urlPrompt.Request("Download URL (leave blank to cancel)", "");
                if (string.IsNullOrWhiteSpace(url))
                {
                    break;
                }

                var manifestBuilder2 = await _xRayClient.GetInstallerBuilder(new Uri(url));
                await _installerBuilder.Compose(manifestBuilder2, true);
                manifestBuilder.Installers.Add(manifestBuilder2);
            }


            _packageManifestService.PrintManifest(manifestBuilder.Build());
            _packageManifestService.WriteManifest(manifestBuilder);
        }
    }
}