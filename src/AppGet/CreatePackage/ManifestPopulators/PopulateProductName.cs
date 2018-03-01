using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AppGet.CommandLine.Prompts;
using AppGet.CreatePackage.Parsers;
using AppGet.Manifests;

namespace AppGet.CreatePackage.ManifestPopulators
{
    public class PopulateProductName : IPopulateManifest
    {
        private readonly TextPrompt _prompt;

        private static readonly Regex NameCleanUp = new Regex("\\(.+\\)|_|\\.|setup|installer|", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex SpaceCleanUp = new Regex("\\s+|\\s\\W\\s", RegexOptions.Compiled);
        private static readonly TextInfo TextInfo = new CultureInfo("en-US", false).TextInfo;

        public PopulateProductName(TextPrompt prompt)
        {
            _prompt = prompt;
        }



        public void Populate(PackageManifest manifest, FileVersionInfo fileVersionInfo, bool interactive)
        {
            string defaultValue = null;

            if (fileVersionInfo != null)
            {
                defaultValue = new[] { fileVersionInfo.ProductName, fileVersionInfo.InternalName, fileVersionInfo.CompanyName }
                    .FirstOrDefault(c => !string.IsNullOrWhiteSpace(c))?.Trim();

                if (defaultValue != null)
                {
                    defaultValue = NameCleanUp.Replace(defaultValue, "");

                    var version = VersionParser.Parse(defaultValue);

                    if (version != null)
                    {
                        defaultValue = defaultValue.Replace(version, "");
                    }
                }
            }


            if (string.IsNullOrWhiteSpace(defaultValue))
            {
                var filePath = manifest.Installers.First().FilePath;
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                var idInName = Regex.Match(fileName, manifest.Id, RegexOptions.IgnoreCase);

                var result = idInName.Captures.Cast<Capture>().FirstOrDefault(c => c.Value != c.Value.ToLower());
                defaultValue = result?.Value ?? TextInfo.ToTitleCase(manifest.Name.Replace("-", " "));
            }
            else
            {
                defaultValue = SpaceCleanUp.Replace(defaultValue, " ");
            }

            manifest.Name = _prompt.Request("Product Name", defaultValue.Trim(), interactive);
        }
    }
}