using AppGet.Manifest;
using AppGet.Windows.WindowsInstaller;

namespace AppGet.Installers
{
    public class InstallerContextFactory
    {
        private readonly WindowsInstallerClient _windowsInstallerClient;
        private readonly NexClient _nexClient;

        public InstallerContextFactory(WindowsInstallerClient windowsInstallerClient, NexClient nexClient)
        {
            _windowsInstallerClient = windowsInstallerClient;
            _nexClient = nexClient;
        }


        public InstallerContext Build(PackageManifest manifest, InstallInteractivityLevel interactivityLevel, PackageOperation operation)
        {
            return new InstallerContext(manifest.Id, interactivityLevel, _windowsInstallerClient, _nexClient)
            {
                InstallerRecords = _windowsInstallerClient.GetRecords(),
                Operation = operation
            };
        }


        public InstallerContext Build(string packageId, InstallInteractivityLevel interactivityLevel, PackageOperation operation)
        {
            return new InstallerContext(packageId, interactivityLevel, _windowsInstallerClient, _nexClient)
            {
                InstallerRecords = _windowsInstallerClient.GetRecords(),
                Operation = operation
            };
        }
    }
}