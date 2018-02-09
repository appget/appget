using System.Collections.Generic;
using AppGet.Manifests;

namespace AppGet.InstalledPackages
{
    public class InstalledPackage
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public InstallMethodType InstallMethod { get; set; }
        public ArchitectureTypes Architecture { get; set; }
        public List<string> ProductIds { get; set; }

        public InstalledPackage()
        {
            ProductIds = new List<string>();
        }
    }
}
