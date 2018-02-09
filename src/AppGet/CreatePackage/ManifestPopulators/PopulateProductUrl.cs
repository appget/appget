using System;
using System.Diagnostics;
using System.Linq;
using AppGet.CommandLine.Prompts;
using AppGet.CreatePackage.Parsers;
using AppGet.Manifests;

namespace AppGet.CreatePackage.ManifestPopulators
{
    public class PopulateProductUrl : IPopulateManifest
    {
        private readonly IUrlPrompt _prompt;

        public PopulateProductUrl(IUrlPrompt prompt)
        {
            _prompt = prompt;
        }

        public void Populate(PackageManifest manifest, FileVersionInfo fileVersionInfo)
        {
            string defaultValue;
            var url = manifest.Installers.First().Location;


            var githubUrl = new GithubUrl(url);
            if (githubUrl.IsValid)
            {
                defaultValue = githubUrl.RepositoryUrl;
            }
            else
            {
                var uri = new Uri(url);
                defaultValue = $"{uri.Scheme}://{uri.Host.Replace("download.", "")}";
            }

            defaultValue = defaultValue.Trim().ToLowerInvariant();

            manifest.ProductUrl = _prompt.Request("Product homepage", defaultValue).ToLowerInvariant();
        }
    }
}

