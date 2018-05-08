using System;
using System.Linq;
using AppGet.CommandLine.Prompts;
using AppGet.CreatePackage;
using AppGet.CreatePackage.Installer;
using AppGet.Manifests;
using AppGet.Manifests.Submission;
using NLog;

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
        private readonly ISubmissionClient _submissionClient;
        private readonly Logger _logger;

        public CreateManifestCommandHandler(IComposeInstaller installerBuilder, IPackageManifestService packageManifestService,
            IComposeManifest composeManifest, IUrlPrompt urlPrompt, IXRayClient xRayClient, BooleanPrompt booleanPrompt, ISubmissionClient submissionClient,
            Logger logger)
        {
            _installerBuilder = installerBuilder;
            _packageManifestService = packageManifestService;
            _composeManifest = composeManifest;
            _urlPrompt = urlPrompt;
            _xRayClient = xRayClient;
            _booleanPrompt = booleanPrompt;
            _submissionClient = submissionClient;
            _logger = logger;
        }

        public bool CanExecute(AppGetOption commandOptions)
        {
            return commandOptions is CreateManifestOptions;
        }

        public void Execute(AppGetOption appGetOption)
        {
            var createOptions = (CreateManifestOptions)appGetOption;

            if (!Uri.IsWellFormedUriString(createOptions.DownloadUrl, UriKind.Absolute))
            {
                throw new InvalidCommandParamaterException("Invalid download URL. Make sure you enter a valid fully qualified download URL.", createOptions);
            }

            var manifestBuilder = _xRayClient.GetBuilder(new Uri(createOptions.DownloadUrl));

            _installerBuilder.Compose(manifestBuilder.Installers.Single());

            _composeManifest.Compose(manifestBuilder, true);

            while (_booleanPrompt.Request("Add an additional installer for different architecture or version of Windows?", false))
            {
                var url = _urlPrompt.Request("Download URL (leave blank to cancel)", "");
                if (string.IsNullOrWhiteSpace(url))
                {
                    break;
                }

                var manifestBuilder2 = _xRayClient.GetInstallerBuilder(new Uri(url));
                _installerBuilder.Compose(manifestBuilder2);
                manifestBuilder.Installers.Add(manifestBuilder2);
            }


            _packageManifestService.PrintManifest(manifestBuilder.Build());
            _packageManifestService.WriteManifest(manifestBuilder);

            var submit = _booleanPrompt.Request("Submit manifest to be reviewed and added to official repository?", true);

            if (submit)
            {
                try
                {
                    var resp = _submissionClient.Submit(manifestBuilder);
                    _logger.Info("Thank you for your submission.");
                    _logger.Info("");
                    _logger.Info("Your pull-request: " + resp.PullRequest);
                }
                catch (Exception e)
                {
                    _logger.Error(e, "Couldn't submit manifest");
                }
            }
        }
    }
}