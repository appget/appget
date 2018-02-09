using System;
using System.Linq;
using AppGet.CommandLine.Prompts;
using AppGet.CreatePackage.Utils;
using AppGet.Manifests;

namespace AppGet.CreatePackage.Populators
{
    public class PopulateProductName : IPopulateManifest
    {
        private readonly IPrompt _prompt;

        public PopulateProductName(IPrompt prompt)
        {
            _prompt = prompt;
        }

        public void Populate(PackageManifest manifest)
        {
            var url = manifest.Installers.First().Location;

            string defaultValue = null;

            var githubUrl = new GithubUrl(url);
            if (githubUrl.IsValid)
            {
                defaultValue = githubUrl.OrganizationName;
            }
            else
            {
                var uri = new Uri(manifest.ProductUrl);
                defaultValue = uri.Host.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries).First();
            }

            manifest.Name = _prompt.Request("Product Name", defaultValue).ToLowerInvariant();
        }
    }
}