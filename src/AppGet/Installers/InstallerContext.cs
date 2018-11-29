using System;
using System.Collections.Generic;
using System.Diagnostics;
using AppGet.Windows.WindowsInstaller;

namespace AppGet.Installers
{
    public class InstallerContext : IDisposable
    {
        private readonly string _packageId;
        public InstallInteractivityLevel InteractivityLevel { get; set; }

        public InstallerContext(string packageId, InstallInteractivityLevel interactivityLevel)
        {
            _packageId = packageId;
            InteractivityLevel = interactivityLevel;
        }

        public Process Process { get; set; }
        public List<WindowsInstallerRecord> InstallerRecords { get; set; }
        public InstallerException Exception { get; set; }
        public PackageOperation Operation { get; set; }

        public void Dispose()
        {
        }
    }
}
