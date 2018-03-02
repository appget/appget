using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AppGet.Manifests;

namespace AppGet.CreatePackage
{
    public enum Confidence
    {
        None,
        LastEffort,
        Plausible,
        Authoritative
    }

    [DebuggerDisplay("{Value} [{Source}:{Confidence}]")]
    public class ManifestAttributeCandidate<T>
    {
        public T Value { get; }
        public Confidence Confidence { get; }
        public string Source { get; }

        public ManifestAttributeCandidate(T value, Confidence confidence, object source)
        {
            if (value is string)
            {
                var str = value.ToString().Trim();

                if (string.IsNullOrWhiteSpace(str))
                {
                    confidence = Confidence.None;
                    str = null;
                }

                value = (dynamic)str;
            }

            Source = source.GetType().Name;
            Value = value;
            Confidence = confidence;
        }
    }

    [DebuggerDisplay("{Value} [{Values.Count}]")]
    public class ManifestAttribute<T>
    {
        public List<ManifestAttributeCandidate<T>> Values { get; }

        public ManifestAttribute()
        {
            Values = new List<ManifestAttributeCandidate<T>>();
        }

        public T Add(T value, Confidence confidence, object source)
        {
            var attr = new ManifestAttributeCandidate<T>(value, confidence, source);
            Values.Add(attr);
            return attr.Value;
        }

        private ManifestAttributeCandidate<T> GetTop()
        {
            return Values
                 .OrderByDescending(c => c.Confidence)
                 .FirstOrDefault(c => c.Confidence > Confidence.None);
        }

        public T Value
        {
            get
            {
                var topAttr = GetTop();
                return topAttr == null ? default(T) : topAttr.Value;
            }
        }

        public bool HasConfidence(Confidence confidence)
        {
            return GetTop()?.Confidence >= confidence;
        }
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
        public ManifestAttribute<string> Licence { get; }


        public ManifestAttribute<InstallMethodTypes> InstallMethod { get; }

        public List<InstallerBuilder> Installers { get; }

        public Uri Uri => new Uri(Installers.First().Location);
        public string FilePath => Installers.First().FilePath;

        public PackageManifestBuilder()
        {
            Id = new ManifestAttribute<string>();
            Name = new ManifestAttribute<string>();
            Version = new ManifestAttribute<string>();
            Home = new ManifestAttribute<string>();
            Repo = new ManifestAttribute<string>();
            Licence = new ManifestAttribute<string>();
            InstallMethod = new ManifestAttribute<InstallMethodTypes>();
            Installers = new List<InstallerBuilder>();
        }


        public PackageManifest Build()
        {
            return new PackageManifest
            {
                Id = Id.Value,
                Name = Name.Value,
                Version = Version.Value,

                Home = Home.Value,
                Repo = Repo.Value,
                Licence = Licence.Value,

                InstallMethod = InstallMethod.Value,
                Installers = Installers.Select(c => c.Build()).ToList(),
            };
        }
    }

    [DebuggerDisplay("{Architecture.Value} {Location} {MinWindowsVersion.Value}]")]
    public class InstallerBuilder
    {
        public string Location { get; set; }
        public string FilePath { get; set; }

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