using System.Diagnostics;
using AppGet.Manifests;

namespace AppGet.CreatePackage.ManifestPopulators
{
    public interface IPopulateManifest
    {
        void Populate(PackageManifest manifest, FileVersionInfo fileVersionInfo);
    }
}