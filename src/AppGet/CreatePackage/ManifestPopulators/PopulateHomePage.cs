using System;
using System.Diagnostics;
using System.Linq;
using AppGet.CommandLine.Prompts;
using AppGet.CreatePackage.Parsers;
using AppGet.Manifests;

namespace AppGet.CreatePackage.ManifestPopulators
{
    public class PopulateHomePage : IPopulateManifest
    {
        private readonly IUrlPrompt _prompt;


        public PopulateHomePage(IUrlPrompt prompt)
        {
            _prompt = prompt;
        }

        public void Populate(PackageManifest manifest, FileVersionInfo fileVersionInfo, bool interactive)
        {
            if (manifest.Home != null) return;

            var uri = new Uri(manifest.Installers.First().Location);

            var defaultValue = HomepageParser.Parse(uri);

            defaultValue = defaultValue?.Trim().ToLowerInvariant();

            manifest.Home = _prompt.Request("Product Homepage", defaultValue, interactive)?.ToLowerInvariant();
        }
    }
}

