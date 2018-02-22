using AppGet.Manifests;

namespace AppGet.CreatePackage.InstallerPopulators
{
    public interface IPopulateInstaller
    {
        void Populate(Installer installer);
    }
}