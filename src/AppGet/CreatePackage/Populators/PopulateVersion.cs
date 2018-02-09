using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using AppGet.CommandLine.Prompts;
using AppGet.Extensions;
using AppGet.Manifests;

namespace AppGet.CreatePackage.Populators
{
    public class PopulateVersion : IPopulateManifest
    {
        private readonly IPrompt _prompt;
        private readonly Regex versionRegex = new Regex("(\\d+\\.)?(\\d+\\.)?(\\*|\\d+)");


        public PopulateVersion(IPrompt prompt)
        {
            _prompt = prompt;
        }

        public void Populate(PackageManifest manifest, FileVersionInfo fileVersionInfo)
        {
            string defaultValue = null;

            if (fileVersionInfo != null)
            {
                defaultValue = new[] { fileVersionInfo.ProductVersion, fileVersionInfo.FileVersion }
                    .FirstOrDefault(c => !string.IsNullOrWhiteSpace(c));
            }

            if (string.IsNullOrWhiteSpace(defaultValue) || defaultValue == "1.0.0")
            {
                var matched = versionRegex.Matches(manifest.Installers.First().Location);

                var bestMatch = matched.Cast<Match>().OrderByDescending(c => c.Length).FirstOrDefault();

                defaultValue = bestMatch?.Value;
            }

            manifest.Version = _prompt.Request("Application Version", defaultValue).ToLowerInvariant();
        }
    }
}

