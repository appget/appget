using System.Diagnostics;
using AppGet.Manifests;

namespace AppGet.CreatePackage.Populators
{
    public interface IPopulateManifest
    {
        void Populate(PackageManifest manifest, FileVersionInfo fileVersionInfo);
    }
}