using AppGet.CreatePackage.Parsers;
using AppGet.Github.Repository;
using AppGet.Manifests;
using NLog;
using System;
using System.Diagnostics;
using System.Linq;

namespace AppGet.CreatePackage.ManifestPopulators
{
    public class GithubPopulater : IPopulateManifest
    {
        private readonly IGitHubRepositoryClient _repositoryClient;
        private readonly Logger _logger;


        public GithubPopulater(IGitHubRepositoryClient repositoryClient, Logger logger)
        {
            _repositoryClient = repositoryClient;
            _logger = logger;
        }

        public void Populate(PackageManifest manifest, FileVersionInfo fileVersionInfo, bool interactive)
        {
            var url = manifest.Installers.First().Location;

            var githubUrl = new GithubUrl(url);
            if (!githubUrl.IsValid) return;

            manifest.Repo = githubUrl.RepositoryUrl;
            manifest.Name = githubUrl.Repository;

            try
            {
                var repo = _repositoryClient.Get(githubUrl.Owner, githubUrl.Repository).Result;
                if (Uri.IsWellFormedUriString(repo.homepage, UriKind.Absolute))
                {
                    manifest.Home = repo.homepage.Trim();
                }

                manifest.Name = repo.name;
                manifest.Licence = repo.license.name;
            }
            catch (Exception e)
            {
                _logger.Error(e, "Couldn't get github repository information.");
            }

        }
    }
}

