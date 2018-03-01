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
        private readonly TextPrompt _prompt;

        public PopulateVersion(TextPrompt prompt)
        {
            _prompt = prompt;
        }

        public void Populate(PackageManifest manifest, FileVersionInfo fileVersionInfo, bool interactive)
        {
            string defaultValue = null;


            if (fileVersionInfo != null)
            {
                defaultValue = new[] { fileVersionInfo.ProductVersion, fileVersionInfo.FileVersion }
                    .FirstOrDefault(c => !string.IsNullOrWhiteSpace(c))?.Trim().Replace(",", ".");
            }

            var urlVersion = VersionParser.Parse(new Uri(manifest.Installers.First().Location));
            if (string.IsNullOrWhiteSpace(manifest.Version) && (urlVersion ?? "").Length > (defaultValue ?? "").Length)
            {
                defaultValue = urlVersion;
            }

            if (defaultValue == null || manifest.Version?.Length > defaultValue.Length)
            {
                defaultValue = manifest.Version;
            }

            manifest.Version = _prompt.Request("Application Version", defaultValue, interactive)?.ToLowerInvariant();
        }
    }
}

