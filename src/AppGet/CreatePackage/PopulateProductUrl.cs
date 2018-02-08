using System;
using System.Linq;
using AppGet.CommandLine;
using AppGet.CreatePackage.Utils;
using AppGet.Manifests;

namespace AppGet.CreatePackage
{
    public interface IPopulateManifest
    {
        void Populate(PackageManifest manifest);
    }

    public class PopulateProductUrl : IPopulateManifest
    {
        private readonly IPrompt _prompt;

        public PopulateProductUrl(IPrompt prompt)
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
                defaultValue = $"{uri.Scheme}://{uri.Host}";
            }

            defaultValue = defaultValue.Trim().ToLowerInvariant();

            manifest.ProductUrl = _prompt.Request("Please enter the product URL", defaultValue).ToLowerInvariant();
        }
    }
}

