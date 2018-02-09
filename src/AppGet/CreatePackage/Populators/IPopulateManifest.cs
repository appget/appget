using AppGet.Manifests;

namespace AppGet.CreatePackage.Populators
{
    public interface IPopulateManifest
    {
        void Populate(PackageManifest manifest);
    }
}