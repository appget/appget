using System;
using AppGet.CreatePackage.Parsers;
using AppGet.Github.Repository;
using NLog;

namespace AppGet.CreatePackage.Root.Extractors
{
    public class GithubExtractor : IExtractToManifestRoot
    {
        private readonly IGitHubRepositoryClient _repositoryClient;
        private readonly Logger _logger;


        public GithubExtractor(IGitHubRepositoryClient repositoryClient, Logger logger)
        {
            _repositoryClient = repositoryClient;
            _logger = logger;
        }

        public void Invoke(PackageManifestBuilder manifestBuilder)
        {
            var githubUrl = new GithubUrl(manifestBuilder.Uri.ToString());
            if (!githubUrl.IsValid) return;

            manifestBuilder.Repo.Add(githubUrl.RepositoryUrl, Confidence.Authoritative, this);
            manifestBuilder.Name.Add(githubUrl.Repository, Confidence.LastEffort, this);

            try
            {
                var repo = _repositoryClient.Get(githubUrl.Owner, githubUrl.Repository).Result;
                if (Uri.IsWellFormedUriString(repo.homepage, UriKind.Absolute))
                {
                    manifestBuilder.Home.Add(repo.homepage, Confidence.Authoritative, this);
                }

                if (repo.license != null && repo.license.key != "other")
                {
                    manifestBuilder.Licence.Add(repo.license.name, Confidence.Authoritative, this);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "Couldn't get github repository information.");
            }
        }
    }
}

