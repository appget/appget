using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace AppGet.Manifests
{
    public class PackageManifest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }

        [YamlIgnore]
        public string MajorVersion { get; set; }
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
        public string ZipSubDir { get; set; }

        public string Sha1 { get; set; }
        public string Sha256 { get; set; }
        public string Md5 { get; set; }

        public ArchitectureTypes Architecture { get; set; }

        public WindowsVersion MinWindowsVersion { get; set; }
        public WindowsVersion MaxWindowsVersion { get; set; }
        public DotNetVersion MinDotNet { get; set; }

        public List<string> ProductIds { get; set; }

        public FileHash GetFileHash()
        {
            if (!string.IsNullOrEmpty(Sha256))
            {
                return new FileHash { HashType = HashType.Sha256, Value = Sha256 };
            }
            if (!string.IsNullOrEmpty(Sha256))
            {
                return new FileHash { HashType = HashType.Sha1, Value = Sha1 };
            }
            if (!string.IsNullOrEmpty(Sha256))
            {
                return new FileHash { HashType = HashType.Md5, Value = Md5 };
            }

            return null;
        }
    }

    public class FileHash
    {
        public HashType HashType { get; set; }
        public string Value { get; set; }
    }


    public enum HashType
    {
        Crc,
        Md5,
        Sha1,
        Sha256,
    }

    public enum ArtifactTypes
    {
        Exe,
        Font,
        PowerShellModule,
        FirefoxExtension,
        ChromeExtension,
        Path,
        EnvironmentVariable
    }


    public enum DotNetVersion
    {
        Unknown = -1,
        None = 0,
        Net10 = 100,
        Net20 = 200,
        Net30 = 300,
        Net35Client = 350,
        Net35 = 351,
        Net40Client = 400,
        Net40 = 401,
        Net45 = 450,
        Net451 = 451,
        Net452 = 452,
        Net460 = 460,
        Net461 = 461,
        Net470 = 470,
        Net471 = 471,
        Core100 = 1000,
        Core200 = 2000
    }

    public enum ArchitectureTypes
    {
        Unknown,
        Any,
        x86,
        x64,
        Itanium,
        ARM
    }

    public enum InstallMethodType
    {
        Unknown = -1,
        Zip,
        MSI,
        Inno,
        InstallShield,
        ClickOnce,
        NSIS,
        Electron
    }
}