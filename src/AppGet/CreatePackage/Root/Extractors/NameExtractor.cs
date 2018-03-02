using System.Text.RegularExpressions;

namespace AppGet.CreatePackage.Root.Extractors
{
    public class NameExtractor : IExtractToManifestRoot
    {
        private readonly Regex _idRegex = new Regex("\\W+");

        public void Invoke(PackageManifestBuilder manifestBuilder)
        {
            var id = _idRegex.Replace(manifestBuilder.Name.Top, "-").ToLowerInvariant().Trim('-');
            manifestBuilder.Id.Add(id.Replace("+", "plus"), Confidence.Reasonable, this);
        }
    }
}