using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AppGet.Manifest.Tests
{
    [TestFixture]
    public class PackageManifestTests
    {
        [Test]
        public void print_sample_manifest()
        {
            var manifest = new PackageManifest
            {
                Id = "linqpad",
                Version = "4.51.03",
                Exe = new[] { "LINQPad.exe" },
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



        [TestCase("name", null, ExpectedResult = "name")]
        [TestCase("name", "latest", ExpectedResult = "name")]
        [TestCase("name", "LATEST", ExpectedResult = "name")]
        [TestCase("name", "", ExpectedResult = "name")]
        [TestCase("name", "1", ExpectedResult = "name_1")]
        public string get_filename(string id, string tag)
        {
            var manifest = new PackageManifest
            {
                Id = id,
                Tag = tag
            };

            return manifest.GetFileName();
        }
    }
}