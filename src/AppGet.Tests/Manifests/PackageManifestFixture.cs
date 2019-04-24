using System;
using System.Collections.Generic;
using System.Linq;
using AppGet.Manifest;
using AppGet.Manifest.Serialization;
using NUnit.Framework;

namespace AppGet.Tests.Manifests
{
    [TestFixture]
    public class PackageManifestFixture : TestBase<object>
    {
        [Test]
        public void print_sample_manifest()
        {
            var manifest = new PackageManifest
            {
                Id = "linqpad",
                Version = "4.51.03",
                Home = "http://www.linqpad.net/",
                InstallMethod = InstallMethodTypes.Zip,
                Installers = new List<Installer>
                {
                    new Installer
                    {
                        Location = "http://www.linqpad.net/GetFile.aspx?LINQPad4-AnyCPU.zip",
                        Architecture = ArchitectureTypes.x86
                    },
                      new Installer
                    {
                        Location = "http://www.linqpad.net/GetFile.aspx?LINQPad4-AnyCPU.zip",
                        Architecture = ArchitectureTypes.x86,
                        MinWindowsVersion = WindowsVersion.KnownVersions.First()
                    }
                }
            };


            Console.WriteLine(manifest.ToYaml());
        }


        [Test]
        public void read_sample_manifest()
        {
            var text = ReadAllText("Manifests\\SampleManifests\\mongodb.yaml");
            Yaml.Deserialize<PackageManifest>(text);
        }
    }
}