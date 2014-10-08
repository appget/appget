using System;

namespace AppGet.FlightPlans
{
    public class FlightPlan
    {
        public string Id { get; set; }
        public string Version { get; set; }
        public Uri Url { get; set; }
        public string Sha256 { get; set; }

        public string[] Exe { get; set; }
        public InstallerType Installer { get; set; }

        public PackageSource[] Packages { get; set; }
    }

    public class PackageSource
    {
        public string Source { get; set; }
        public ArchitectureType Architecture { get; set; }

        //http://en.wikipedia.org/wiki/List_of_Microsoft_Windows_versions
        public double MinWindowsVersion { get; set; }
        public double MaxWindowsVersion { get; set; }
    }

    public enum ArchitectureType
    {
        x86,
        AMD64,
        IA64
    }

    public enum InstallerType
    {
        Zip,
        MSI,
        Inno,
        InstallShield,
        NSIS
    }
}