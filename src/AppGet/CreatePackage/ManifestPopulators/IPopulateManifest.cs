using System.Diagnostics;

namespace AppGet.CreatePackage.ManifestPopulators
{
    public interface IPopulateManifest
    {
        void Populate(PackageManifestBuilder manifestBuilder, FileVersionInfo fileVersionInfo, bool interactive);
    }
}