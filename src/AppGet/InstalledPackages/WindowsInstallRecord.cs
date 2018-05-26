using System.Diagnostics;
using AppGet.Manifest;
using AppGet.Manifests;
using Microsoft.Win32;

namespace AppGet.InstalledPackages
{
    [DebuggerDisplay("{" + nameof(Name) + "} " + "{" + nameof(Id) + "}")]
    public class WindowsInstallRecord
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string UninstallCommand { get; set; }
        public string InstallLocation { get; set; }
        public string Publisher { get; set; }
        public InstallMethodTypes InstallMethod { get; set; }
        public string QuietUninstallCommand { get; set; }
        public string InstallDate { get; set; }
        public string InstallSource { get; set; }
        public RegistryKey RegistryKey { get; set; }
    }
}