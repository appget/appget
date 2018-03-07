using System.Text.RegularExpressions;

namespace AppGet.CreatePackage.Root.Extractors
{
    public class NameExtractor : IExtractToManifestRoot
    {
        private readonly Regex _idRegex = new Regex("\\W+");

        public void Invoke(PackageManifestBuilder manifestBuilder)
        {
            var name = manifestBuilder.Name.Value;

            if (string.IsNullOrWhiteSpace(name)) return;

            var id = _idRegex.Replace(name, "-").ToLowerInvariant().Trim('-');
            manifestBuilder.Id.Add(id.Replace("+", "plus"), Confidence.Plausible, this);
        }
    }
}