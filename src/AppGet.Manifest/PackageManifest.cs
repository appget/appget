using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AppGet.Manifest.Serialization;
using Newtonsoft.Json;
using YamlDotNet.Serialization;

namespace AppGet.Manifest
{
    [DebuggerDisplay("{" + nameof(Id) + "} " + "{" + nameof(Version) + "}")]
    public class PackageManifest
    {
        public const string LATEST_TAG = "latest";

        public string Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }

        public string Home { get; set; }
        public string Repo { get; set; }
        public string License { get; set; }

        public InstallMethodTypes InstallMethod { get; set; }
        public InstallArgs Args { get; set; }

        public List<Installer> Installers { get; set; }

        [YamlIgnore]
        public string Tag { get; set; }

        [YamlIgnore]
        [JsonIgnore]
        public bool IsLatest => TagHelper.IsLatest(Tag);

        public PackageManifest()
        {
            InstallMethod = InstallMethodTypes.Custom;
            Installers = new List<Installer>();
        }

        public PackageManifest(PackageManifest manifest)
        {
            Id = manifest.Id;
            Name = manifest.Name;
            Version = manifest.Version;

            Home = manifest.Home;
            Repo = manifest.Repo;
            License = manifest.License;

            InstallMethod = manifest.InstallMethod;
            Args = manifest.Args;

            Installers = manifest.Installers.Select(i => new Installer
            {
                Architecture = i.Architecture,
                Location = i.Location,
                Sha256 = i.Sha256
            }).ToList();

            Tag = manifest.Tag;
        }

        public override string ToString()
        {
            return $"{Id} {Version}".Trim();
        }

        public virtual string ToYaml()
        {
            // make sure we aren't serializing child classes.
            if (this.GetType() != typeof(PackageManifest))
            {
                return new PackageManifest(this).ToYaml();
            }

            return Yaml.Serialize(this);
        }

        public string GetFileName()
        {
            var tag = Tag?.Trim().ToLower();
            var id = Id.Trim().ToLower();
            return IsLatest ? $"{id}" : $"{id}_{tag}";
        }
    }

    public class InstallArgs
    {
        public string Passive { get; set; }
        public string Silent { get; set; }
        public string Interactive { get; set; }
        public string Log { get; set; }
    }

    public class Installer
    {
        public string Location { get; set; }
        public string Sha256 { get; set; }
        public ArchitectureTypes Architecture { get; set; }

        public Version MinWindowsVersion { get; set; }
    }
}
