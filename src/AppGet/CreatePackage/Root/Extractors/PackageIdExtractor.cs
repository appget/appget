using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AppGet.Extensions;

namespace AppGet.CreatePackage.Root.Extractors
{
    public class PackageIdExtractor : IExtractToManifestRoot
    {
        public void Invoke(PackageManifestBuilder manifest)
        {
            var fileName = Path.GetFileNameWithoutExtension(manifest.FilePath);
            var idInName = Regex.Match(fileName, manifest.Id.Top, RegexOptions.IgnoreCase);

            var nameInFileName = idInName.Captures.Cast<Capture>()
                .FirstOrDefault(c => c.Value != c.Value.ToLower());

            if (!string.IsNullOrWhiteSpace(nameInFileName?.Value))
            {
                manifest.Name.Add(nameInFileName.Value, Confidence.Reasonable, this);
            }

            var nameFromId = manifest.Id.Top.Replace("-", " ").ToTitleCase();

            manifest.Name.Add(nameFromId, Confidence.LastEffort, this);
        }
    }
}