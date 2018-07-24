using AppGet.Commands.Install;
using AppGet.Manifest;

namespace AppGet.Installers
{
    public interface IInstallerWhisperer
    {
        void Install(string installerLocation, PackageManifest packageManifest, InstallOptions installOptions);
        bool CanHandle(InstallMethodTypes installMethod);
        InstallMethodTypes InstallMethod { get; }
    }
}