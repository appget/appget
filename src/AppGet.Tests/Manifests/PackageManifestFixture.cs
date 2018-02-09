using System;
using System.Collections.Generic;
using AppGet.Manifests;
using AppGet.Serialization;
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
                Exe = new[] { "LINQPad.exe" },
                ProductUrl = "http://www.linqpad.net/",
                InstallMethod = InstallMethodType.Zip,
                Installers = new List<Installer>
                {
                    new Installer
                    {
                        Location = "http://www.linqpad.net/GetFile.aspx?LINQPad4-AnyCPU.zip",
                        Architecture = ArchitectureTypes.Unknown
                    },
                      new Installer
                    {
                        Location = "http://www.linqpad.net/GetFile.aspx?LINQPad4-AnyCPU.zip",
                        Architecture = ArchitectureTypes.Unknown,
                        MinWindowsVersion = WindowsVersion.VistaSp2,
                        MaxWindowsVersion = WindowsVersion.Eight
                    }
                }
            };


            Console.WriteLine(Yaml.Serialize(manifest));
        }


        [Test]
        public void read_sample_manifest()
        {
            var text = ReadAllText("Manifests\\SampleManifests\\mongodb.yaml");
            Yaml.Deserialize<PackageManifest>(text);
        }
    }
}