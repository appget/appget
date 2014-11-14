using AppGet.Commands.Install;
using AppGet.Commands.Uninstall;
using AppGet.Manifests;

namespace AppGet.Installers
{
    public interface IInstallerWhisperer
    {
        void Install(string installerLocation, PackageManifest packageManifest, InstallOptions installOptions);
        void Uninstall(PackageManifest packageManifest, UninstallOptions installOptions);
        bool CanHandle(InstallMethodType installMethod);
    }
}