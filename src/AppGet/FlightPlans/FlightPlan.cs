using System;
using System.Collections.Generic;

namespace AppGet.FlightPlans
{
    public class FlightPlan
    {
        public string Id { get; set; }
        public string Version { get; set; }
        public string ProductUrl { get; set; }

        public string[] Exe { get; set; }
        public InstallMethodType InstallMethod { get; set; }

        public List<Installer> Installers { get; set; }
    }


    public class Artifact
    {
        public ArtifactTypes Type { get; set; }
        public string Path { get; set; }
    }

    public class Installer
    {
        public string Location { get; set; }
        public string FileName { get; set; }
        public string Sha1 { get; set; }
        public string Sha256 { get; set; }
        public string Md5 { get; set; }

        public ArchitectureType Architecture { get; set; }

        //http://en.wikipedia.org/wiki/List_of_Microsoft_Windows_versions
        public Version MinWindowsVersion { get; set; }
        public Version MaxWindowsVersion { get; set; }
        public DotNetVersion MinDotNet { get; set; }
    }

    public enum ArtifactTypes
    {
        Exe,
        Font,
        PowerShellModule,
        FirefoxExtention,
        ChromeExtention,
        Path,
        EnvironmetVarible
    }


    public enum WindowsVersion
    {
        Xp,
        XpSp1,
        XpSp2,
        Vista,
        VistaSp1,
        VistaSp2,
        Seven,
        SevenSp1,
        Eight,
        EightOne
    }

    public enum DotNetVersion
    {
        Net10 = 100,
        Net20 = 200,
        Net30 = 300,
        Net35Client = 350,
        Net35 = 351,
        Net40Client = 400,
        Net40 = 401,
        Net45 = 450
    }

    public enum ArchitectureType
    {
        Any,
        x86,
        x64,
        Itanium,
        ARM
    }

    public enum InstallMethodType
    {
        Zip,
        MSI,
        Inno,
        InstallShield,
        NSIS
    }
}