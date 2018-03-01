using System;
using System.Diagnostics;
using System.Linq;
using AppGet.Manifests;

namespace AppGet.CreatePackage.ManifestPopulators
{
    public class SourceforgePopulater : IPopulateManifest
    {
        public void Populate(PackageManifest manifest, FileVersionInfo fileVersionInfo, bool interactive)
        {
            var uri = new Uri(manifest.Installers.First().Location);

            if (!uri.Host.ToLowerInvariant().Contains("sourceforce.")) return;

            manifest.Repo = $"https://sourceforge.net/projects/{uri.Segments[3]}";
        }
    }
}

