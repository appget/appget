
namespace AppGet.CreatePackage.InstallerPopulators
{
    public interface IPopulateInstaller
    {
        void Populate(InstallerBuilder installerBuilder, bool interactive);
    }
}