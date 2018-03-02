using AppGet.CreatePackage.Parsers;

namespace AppGet.CreatePackage.Root.Extractors
{
    public class UrlExtractor : IExtractToManifestRoot
    {
        public void Invoke(PackageManifestBuilder manifestBuilder)
        {
            var uri = manifestBuilder.Uri;
            manifestBuilder.Home.Add(HomepageParser.Parse(uri), Confidence.LastEffort, this);
            manifestBuilder.Version.Add(VersionParser.Parse(uri), Confidence.Reasonable, this);
        }
    }
}

