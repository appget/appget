using System;
using System.Linq;
using System.Text.RegularExpressions;
using AppGet.CommandLine.Prompts;
using AppGet.CreatePackage.Utils;
using AppGet.Manifests;

namespace AppGet.CreatePackage.Populators
{
    public class PopulateProductUrl : IPopulateManifest
    {
        private readonly IUrlPrompt _prompt;

        public PopulateProductUrl(IUrlPrompt prompt)
        {
            _prompt = prompt;
        }

        public void Populate(PackageManifest manifest)
        {
            var url = manifest.Installers.First().Location;

            string defaultValue;

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

