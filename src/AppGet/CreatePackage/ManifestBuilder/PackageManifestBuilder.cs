using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AppGet.Extensions;
using AppGet.Manifests;
using AppGet.Serialization;
using Newtonsoft.Json;

namespace AppGet.CreatePackage.ManifestBuilder
{
    public enum Confidence
    {
        None,
        LastEffort,
        Plausible,
        Confident,
        Authoritative
    }

    [DebuggerDisplay("{Id} {Version} [{Name}]")]
    public class PackageManifestBuilder
    {
        public string VersionTag { get; set; }

        public ManifestAttribute<string> Id { get; }
        public ManifestAttribute<string> Name { get; }
        public ManifestAttribute<string> Version { get; }

        public ManifestAttribute<string> Home { get; }
        public ManifestAttribute<string> Repo { get; }
        public ManifestAttribute<string> License { get; }


        public ManifestAttribute<InstallMethodTypes> InstallMethod { get; }

        public List<InstallerBuilder> Installers { get; }

        public ManifestAttribute<InstallArgs> Args { get; private set; }

        [JsonIgnore]
        public Uri Uri => new Uri(Installers.First().Location);

        [JsonIgnore]
        public string FilePath { get; set; }

        public PackageManifestBuilder()
        {
            Id = new ManifestAttribute<string>();
            Name = new ManifestAttribute<string>(v => v?.CapitalLettersCount());
            Version = new ManifestAttribute<string>(v => v?.PeriodCount());
            Home = new ManifestAttribute<string>();
            Repo = new ManifestAttribute<string>();
            License = new ManifestAttribute<string>();
            InstallMethod = new ManifestAttribute<InstallMethodTypes>();
            Args = new ManifestAttribute<InstallArgs>();
            Installers = new List<InstallerBuilder>();
        }


        public PackageManifest Build()
        {
            if (InstallMethod.Value == InstallMethodTypes.Squirrel || JsonEquality.Equal(Args.Value, new InstallArgs()))
            {
                Args = null;
            }

            return new PackageManifest
            {
                Id = Id.Value,
                Name = Name.Value,
                Version = Version.Value,

                Home = Home.Value,
                Repo = Repo.Value,
                License = License.Value,

                InstallMethod = InstallMethod.Value,
                Installers = Installers.Select(c => c.Build()).OrderBy(c => c.Architecture).ToList(),
                Args = Args?.Value
            };
        }
    }

    [DebuggerDisplay("{Architecture.Value} {Location} {MinWindowsVersion.Value}]")]
    public class InstallerBuilder
    {
        public string Location { get; set; }
        public string Sha256 { get; set; }
        public ManifestAttribute<ArchitectureTypes> Architecture { get; }
        public ManifestAttribute<Version> MinWindowsVersion { get; }

        public InstallerBuilder()
        {
            Architecture = new ManifestAttribute<ArchitectureTypes>();
            MinWindowsVersion = new ManifestAttribute<Version>();
        }

        public Manifests.Installer Build()
        {
            return new Manifests.Installer
            {
                Location = Location,
                Sha256 = Sha256,
                Architecture = Architecture.Value,
                MinWindowsVersion = MinWindowsVersion.Value
            };
        }
    }
}