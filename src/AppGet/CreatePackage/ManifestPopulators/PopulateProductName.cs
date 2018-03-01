using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using AppGet.CommandLine.Prompts;
using AppGet.CreatePackage.Parsers;
using AppGet.Github.Repository;
using AppGet.Manifests;
using NLog;

namespace AppGet.CreatePackage.ManifestPopulators
{
    public class PopulateProductName : IPopulateManifest
    {
        private readonly TextPrompt _prompt;
        private readonly IGitHubRepositoryClient _repositoryClient;
        private readonly Logger _logger;
        readonly TextInfo _textInfo = new CultureInfo("en-US", false).TextInfo;


        public PopulateProductName(TextPrompt prompt, IGitHubRepositoryClient repositoryClient, Logger logger)
        {
            _prompt = prompt;
            _repositoryClient = repositoryClient;
            _logger = logger;
        }

        public void Populate(PackageManifest manifest, FileVersionInfo fileVersionInfo, bool interactive)
        {
            string defaultValue = null;

            if (fileVersionInfo != null)
            {
                defaultValue = new[] { fileVersionInfo.ProductName, fileVersionInfo.InternalName, fileVersionInfo.CompanyName }
                    .FirstOrDefault(c => !string.IsNullOrWhiteSpace(c))?.Trim();
            }

            if (defaultValue == null)
            {
                var url = manifest.Installers.First().Location;

                var githubUrl = new GithubUrl(url);
                if (githubUrl.IsValid)
                {
                    defaultValue = _textInfo.ToTitleCase(githubUrl.Owner);

                    try
                    {
                        var repo = _repositoryClient.Get(githubUrl.Owner, githubUrl.Repository).Result;
                        defaultValue = repo.name.Trim();
                    }
                    catch (Exception e)
                    {
                        _logger.Error(e, "Couldn't get github repository information.");
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(defaultValue))
            {
                defaultValue = manifest.Name;
            }

            manifest.Name = _prompt.Request("Product Name", defaultValue, interactive);
        }
    }
}