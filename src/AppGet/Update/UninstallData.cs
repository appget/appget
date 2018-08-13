using AppGet.Manifest;

namespace AppGet.Update
{
    public class UninstallData
    {
        public string PackageId { get; set; }
        public string WindowsInstallerId { get; set; }
        public string InstallationPath { get; set; }
        public string DisplayVersion { get; set; }
        public string DisplayName { get; set; }
        public InstallMethodTypes InstallMethod { get; set; }
    }
}