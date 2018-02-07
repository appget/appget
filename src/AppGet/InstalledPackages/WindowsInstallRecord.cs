using AppGet.Manifests;

namespace AppGet.InstalledPackages
{
    public class WindowsInstallRecord
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string UninstallCommand { get; set; }
        public string InstallLocation { get; set; }
        public string Publisher { get; set; }
        public InstallMethodType InstallMethod { get; set; }
        public string QuietUninstallCommand { get; set; }
        public string InstallDate { get; set; }
        public string InstallSource { get; set; }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is WindowsInstallRecord other))
            {
                return false;
            }

            return other.Id == Id;
        }
    }
}
