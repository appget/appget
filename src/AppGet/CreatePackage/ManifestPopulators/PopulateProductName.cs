using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AppGet.CommandLine.Prompts;
using AppGet.CreatePackage.Parsers;
using AppGet.Extensions;

namespace AppGet.CreatePackage.ManifestPopulators
{
    public class PopulateProductName : IPopulateManifest
    {
        private readonly TextPrompt _prompt;

        private static readonly Regex NameCleanUp = new Regex("\\(.+\\)|_|\\.|setup|installer|", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex SpaceCleanUp = new Regex("\\s+|\\s\\W\\s", RegexOptions.Compiled);

        public PopulateProductName(TextPrompt prompt)
        {
            _prompt = prompt;
        }

        public void Populate(PackageManifestBuilder manifest, FileVersionInfo fileVersionInfo, bool interactive)
        {
            if (fileVersionInfo != null)
            {
                var fileVersionName = new[] { fileVersionInfo.ProductName, fileVersionInfo.InternalName, fileVersionInfo.CompanyName }
                    .FirstOrDefault(c => !string.IsNullOrWhiteSpace(c))?.Trim();

                if (fileVersionName != null)
                {
                    fileVersionName = NameCleanUp.Replace(fileVersionName, "");

                    var version = VersionParser.Parse(fileVersionName);

                    if (version != null)
                    {
                        fileVersionName = fileVersionName.Replace(version, "");
                    }
                }

                if (fileVersionName != null)
                {
                    SpaceCleanUp.Replace(fileVersionName, " ");
                    manifest.Name.Add(fileVersionName, Confidence.VeryHigh, this);
                }
            }

            var filePath = manifest.Installers.First().FilePath;
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var idInName = Regex.Match(fileName, manifest.Id.Top, RegexOptions.IgnoreCase);

            var nameInFileName = idInName.Captures.Cast<Capture>().FirstOrDefault(c => c.Value != c.Value.ToLower());
            var nameFromId = nameInFileName?.Value ?? manifest.Id.Top.Replace("-", " ").ToTitleCase();

            manifest.Name.Add(nameFromId, Confidence.Reasonable, this);

            if (!interactive || manifest.Name.HasConfidence(Confidence.VeryHigh)) return;
            manifest.Name.Add(_prompt.Request("Product Name", manifest.Name.Top), Confidence.VeryHigh, this);
        }
    }
}