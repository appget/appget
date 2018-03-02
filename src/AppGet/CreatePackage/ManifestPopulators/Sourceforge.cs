using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using AppGet.Extensions;
using AppGet.Manifests;

namespace AppGet.CreatePackage.ManifestPopulators
{
    public class SourceforgePopulater : IPopulateManifest
    {
        public void Populate(PackageManifestBuilder manifest, FileVersionInfo fileVersionInfo, bool interactive)
        {
            var uri = new Uri(manifest.Installers.First().Location);

            if (!uri.Host.ToLowerInvariant().Contains("sourceforge.")) return;

            var titleSegment = uri.Segments[2];

            manifest.Repo.Add($"https://sourceforge.net/projects/{titleSegment}", Confidence.VeryHigh, this);

            manifest.Name.Add(titleSegment.ToTitleCase(), Confidence.Low, this);
        }
    }
}