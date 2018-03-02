using AppGet.CreatePackage.Parsers;
using AppGet.Github.Repository;
using NLog;
using System;
using System.Diagnostics;

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

        public void Populate(PackageManifestBuilder manifestBuilder, FileVersionInfo fileVersionInfo, bool interactive)
        {
            var githubUrl = new GithubUrl(manifestBuilder.Url.ToString());
            if (!githubUrl.IsValid) return;

            manifestBuilder.Repo.Add(githubUrl.RepositoryUrl, Confidence.VeryHigh, this);
            manifestBuilder.Name.Add(githubUrl.Repository, Confidence.Low, this);

            try
            {
                var repo = _repositoryClient.Get(githubUrl.Owner, githubUrl.Repository).Result;
                if (Uri.IsWellFormedUriString(repo.homepage, UriKind.Absolute))
                {
                    manifestBuilder.Home.Add(repo.homepage, Confidence.VeryHigh, this);
                }

                manifestBuilder.Name.Add(repo.name, Confidence.Reasonable, this);
                manifestBuilder.Licence.Add(repo.license.name, Confidence.VeryHigh, this);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Couldn't get github repository information.");
            }
        }
    }
}

