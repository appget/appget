using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using AppGet.CreatePackage.Parsers;
using AppGet.Extensions;

namespace AppGet.CreatePackage.Root.Extractors
{
    public class FileVersionInfoExtractor : IExtractToManifestRoot
    {
        private static IEnumerable<string> GetValues(params string[] values)
        {
            foreach (var value in values)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    yield return value.Trim();
                }
            }
        }


        public void Invoke(PackageManifestBuilder manifest)
        {
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(manifest.FilePath);

            ExtractVersion(manifest, fileVersionInfo);
            ExtractName(manifest, fileVersionInfo);
        }

        private void ExtractVersion(PackageManifestBuilder manifest, FileVersionInfo fileVersionInfo)
        {
            var fileVersion = GetValues(fileVersionInfo.ProductVersion, fileVersionInfo.FileVersion)
                .Select(c => c.Replace(",", "."))
                .Select(VersionParser.Parse)
                .OrderByDescending(c => c?.PeriodCount())
                .FirstOrDefault();

            manifest.Version.Add(fileVersion, Confidence.Plausible, this);
        }


        private static readonly Regex NameCleanUp = new Regex("\\(.+\\)|_|\\.|setup|installer|update|x?64|x?32\\.exe", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex SpaceCleanUp = new Regex("\\s+|\\s\\W\\s", RegexOptions.Compiled);

        private void ExtractName(PackageManifestBuilder manifest, FileVersionInfo fileVersionInfo)
        {
            var name = GetValues(fileVersionInfo.ProductName, fileVersionInfo.InternalName,
                     fileVersionInfo.FileDescription, fileVersionInfo.CompanyName)
                .Select(c => c.Replace(",", ".").Trim())
                .FirstOrDefault();


            if (name != null)
            {
                name = NameCleanUp.Replace(name, "");

                var version = VersionParser.Parse(name);

                if (version != null)
                {
                    name = name.Replace(version, "");
                }

                SpaceCleanUp.Replace(name, " ");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                manifest.Name.Add(name, Confidence.None, this);
            }
            else
            {
                manifest.Name.Add(name, Confidence.Plausible, this);
            }
        }
    }
}

