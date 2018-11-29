using AppGet.Manifest;
using AppGet.Windows.WindowsInstaller;

namespace AppGet.Installers
{
    public class InstallerContextFactory
    {
        private readonly WindowsInstallerClient _windowsInstallerClient;

        public InstallerContextFactory(WindowsInstallerClient windowsInstallerClient)
        {
            _windowsInstallerClient = windowsInstallerClient;
        }


        public InstallerContext Build(PackageManifest manifest, InstallInteractivityLevel interactivityLevel, PackageOperation operation)
        {
            return new InstallerContext(manifest.Id, interactivityLevel)
            {
                InstallerRecords = _windowsInstallerClient.GetRecords(),
                Operation = operation
            };
        }


        public InstallerContext Build(string packageId, InstallInteractivityLevel interactivityLevel, PackageOperation operation)
        {
            return new InstallerContext(packageId, interactivityLevel)
            {
                InstallerRecords = _windowsInstallerClient.GetRecords(),
                Operation = operation
            };
        }
    }
}