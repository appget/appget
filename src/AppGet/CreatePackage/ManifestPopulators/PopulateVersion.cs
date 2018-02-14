using System;
using System.Diagnostics;
using System.Linq;
using AppGet.CommandLine.Prompts;
using AppGet.CreatePackage.Parsers;
using AppGet.Manifests;

namespace AppGet.CreatePackage.ManifestPopulators
{
    public class PopulateVersion : IPopulateManifest
    {
        private readonly IPrompt<string> _prompt;

        public PopulateVersion(IPrompt<string> prompt)
        {
            _prompt = prompt;
        }

        public void Populate(PackageManifest manifest, FileVersionInfo fileVersionInfo)
        {
            string defaultValue = null;

            var urlVersion = VersionParser.Parse(new Uri(manifest.Installers.First().Location));

            if (fileVersionInfo != null)
            {
                defaultValue = new[] { fileVersionInfo.ProductVersion, fileVersionInfo.FileVersion }
                    .FirstOrDefault(c => !string.IsNullOrWhiteSpace(c));
            }

            if (urlVersion?.Length > defaultValue?.Length)
            {
                defaultValue = urlVersion;
            }

            manifest.Version = _prompt.Request("Application Version", defaultValue).ToLowerInvariant();
        }
    }
}

