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
            if (manifest.Id.Value == null) return;

            var fileName = Path.GetFileNameWithoutExtension(manifest.FilePath);
            var idInName = Regex.Match(fileName, Regex.Escape(manifest.Id.Value), RegexOptions.IgnoreCase);

            var nameInFileName = idInName.Captures.Cast<Capture>()
                .FirstOrDefault(c => c.Value != c.Value.ToLower());

            if (!string.IsNullOrWhiteSpace(nameInFileName?.Value))
            {
                manifest.Name.Add(nameInFileName.Value, Confidence.Plausible, this);
            }

            var nameFromId = manifest.Id.Value.Replace("-", " ").ToTitleCase();

            manifest.Name.Add(nameFromId, Confidence.LastEffort, this);
        }
    }
}