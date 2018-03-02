using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AppGet.Manifests;

namespace AppGet.CreatePackage
{

    public enum Confidence
    {
        NoMatch,
        Low,
        Reasonable,
        VeryHigh
    }

    public class ManifestAttributeCandidate<T>
    {
        public T Value { get; }
        public Confidence Score { get; }
        public string Source { get; }

        public ManifestAttributeCandidate(T value, Confidence score, object source)
        {
            Source = source.GetType().Name;
            Value = value;
            Score = score;
        }
    }

    public class ManifestAttributeStringCandidate : ManifestAttributeCandidate<string>
    {
        public ManifestAttributeStringCandidate(string value, Confidence score, object source)
            : base(value?.Trim(), score, source)
        {
        }
    }

    public class ManifestAttribute<T>
    {
        public List<ManifestAttributeCandidate<T>> Values { get; }

        public ManifestAttribute()
        {
            Values = new List<ManifestAttributeCandidate<T>>();
        }

        public virtual void Add(T value, Confidence scroe, object source)
        {
            Values.Add(new ManifestAttributeCandidate<T>(value, scroe, source));
        }

        public virtual T Top
        {
            get
            {
                if (!Values.Any() || Values.All(c => c.Score == Confidence.NoMatch)) return default(T);
                return Values.OrderByDescending(c => c.Score).FirstOrDefault().Value;
            }
        }

        public bool HasValue => Top != null;

        public bool HasConfidence(Confidence confidence)
        {
            return Values.OrderByDescending(c => c.Score).FirstOrDefault()?.Score >= confidence;
        }
    }

    public class ManifestStringAttribute
    {
        public List<ManifestAttributeStringCandidate> Values { get; }

        public ManifestStringAttribute()
        {
            Values = new List<ManifestAttributeStringCandidate>();
        }


        public string Add(string value, Confidence scroe, object source)
        {
            var candidate = new ManifestAttributeStringCandidate(value, scroe, source);
            Values.Add(candidate);
            return candidate.Value;
        }

        public string Top => Values.OrderByDescending(c => c.Score).FirstOrDefault()?.Value;
        public bool HasValue => Top != null;

        public bool HasConfidence(Confidence confidence)
        {
            return Values.OrderByDescending(c => c.Score).FirstOrDefault()?.Score >= confidence;
        }
    }

    [DebuggerDisplay("{" + nameof(Id) + "} " + "{" + nameof(Version) + "}")]
    public class PackageManifestBuilder
    {
        public string VersionTag { get; set; }

        public ManifestStringAttribute Id { get; }
        public ManifestStringAttribute Name { get; }
        public ManifestStringAttribute Version { get; }

        public ManifestStringAttribute Home { get; }
        public ManifestStringAttribute Repo { get; }
        public ManifestStringAttribute Licence { get; }


        public ManifestAttribute<InstallMethodTypes> InstallMethod { get; }

        public List<InstallerBuilder> Installers { get; }

        public Uri Url => new Uri(Installers.First().Location);
        public string FilePath => Installers.First().FilePath;

        public PackageManifestBuilder()
        {
            Id = new ManifestStringAttribute();
            Name = new ManifestStringAttribute();
            Version = new ManifestStringAttribute();
            Home = new ManifestStringAttribute();
            Repo = new ManifestStringAttribute();
            Licence = new ManifestStringAttribute();
            InstallMethod = new ManifestAttribute<InstallMethodTypes>();
            Installers = new List<InstallerBuilder>();
        }


        public PackageManifest Build()
        {
            return null;
        }
    }


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

    }
}