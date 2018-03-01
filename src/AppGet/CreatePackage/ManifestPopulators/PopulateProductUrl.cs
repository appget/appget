using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using AppGet.CommandLine.Prompts;
using AppGet.CreatePackage.Parsers;
using AppGet.Github.Repository;
using AppGet.Manifests;
using NLog;

namespace AppGet.CreatePackage.ManifestPopulators
{
    public class PopulateProductUrl : IPopulateManifest
    {
        private readonly IUrlPrompt _prompt;
        private readonly IGitHubRepositoryClient _repositoryClient;
        private readonly Logger _logger;

        private static readonly Regex HostNameRegex = new Regex(@"(download|update|mirror|^repo|^dl)\w*\.", RegexOptions.IgnoreCase);
        private static readonly Regex DedicatedFileHost = new Regex(@"(\.s3\.amazonaws\.)|(fosshub\.com)", RegexOptions.IgnoreCase);

        public PopulateProductUrl(IUrlPrompt prompt, IGitHubRepositoryClient repositoryClient, Logger logger)
        {
            _prompt = prompt;
            _repositoryClient = repositoryClient;
            _logger = logger;
        }

        public void Populate(PackageManifest manifest, FileVersionInfo fileVersionInfo, bool interactive)
        {
            string defaultValue;
            var url = manifest.Installers.First().Location;


            var githubUrl = new GithubUrl(url);
            if (githubUrl.IsValid)
            {
                defaultValue = githubUrl.RepositoryUrl;

                try
                {
                    var repo = _repositoryClient.Get(githubUrl.Owner, githubUrl.Repository).Result;
                    if (Uri.IsWellFormedUriString(repo.homepage, UriKind.Absolute))
                    {
                        defaultValue = repo.homepage.Trim();
                    }
                }
                catch (Exception e)
                {
                    _logger.Error(e, "Couldn't get github repository information.");
                }
            }
            else
            {
                var uri = new Uri(url);
                if (DedicatedFileHost.IsMatch(uri.Host))
                {
                    defaultValue = "";
                }
                else
                {
                    defaultValue = $"{uri.Scheme}://{HostNameRegex.Replace(uri.Host, "")}";
                }
            }

            defaultValue = defaultValue.Trim().ToLowerInvariant();

            manifest.ProductUrl = _prompt.Request("Product Homepage", defaultValue, interactive)?.ToLowerInvariant();
        }
    }
}

