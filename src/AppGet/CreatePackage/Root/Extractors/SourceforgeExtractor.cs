using System;
using System.Linq;
using AppGet.Extensions;

namespace AppGet.CreatePackage.Root.Extractors
{
    public class SourceforgeExtractor : IExtractToManifestRoot
    {
        public void Invoke(PackageManifestBuilder manifest)
        {
            var uri = new Uri(manifest.Installers.First().Location);

            if (!uri.Host.ToLowerInvariant().Contains("sourceforge.")) return;

            var titleSegment = uri.Segments[2];

            manifest.Repo.Add($"https://sourceforge.net/projects/{titleSegment}", Confidence.Authoritive, this);

            manifest.Name.Add(titleSegment.ToTitleCase().Trim('/'), Confidence.LastEffort, this);
        }
    }
}