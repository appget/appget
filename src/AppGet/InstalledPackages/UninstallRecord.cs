using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppGet.FlightPlans;

namespace AppGet.InstalledPackages
{
    public class UninstallRecord
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string UninstallCommand { get; set; }
        public string InstallLocation { get; set; }
        public string Publisher { get; set; }
        public InstallMethodType InstallMethod { get; set; }
    }
}
