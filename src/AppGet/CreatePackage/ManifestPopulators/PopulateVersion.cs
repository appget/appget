using System.Diagnostics;
using System.Linq;
using AppGet.CommandLine.Prompts;
using AppGet.CreatePackage.Parsers;

namespace AppGet.CreatePackage.ManifestPopulators
{
    public class PopulateVersion : IPopulateManifest
    {
        private readonly TextPrompt _prompt;

        public PopulateVersion(TextPrompt prompt)
        {
            _prompt = prompt;
        }

        public void Populate(PackageManifestBuilder manifest, FileVersionInfo fileVersionInfo, bool interactive)
        {
            if (fileVersionInfo != null)
            {
                var fileVersion = new[] { fileVersionInfo.ProductVersion, fileVersionInfo.FileVersion }
                    .FirstOrDefault(c => !string.IsNullOrWhiteSpace(c))?.Trim().Replace(",", ".");

                manifest.Version.Add(fileVersion, Confidence.Reasonable, this);
            }

            var urlVersion = VersionParser.Parse(manifest.Url);
            manifest.Version.Add(urlVersion, Confidence.Reasonable, this);

            if (!interactive || manifest.Version.HasConfidence(Confidence.VeryHigh)) return;

            manifest.Version.Add(_prompt.Request("Application Version", manifest.Version.Top), Confidence.Reasonable, this);
        }
    }
}

