using System.IO;
using System.Linq;
using AppGet.Compression;
using AppGet.Installers.Squirrel;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Installers.Squirrel
{
    [TestFixture]
    public class SquirrelReaderFixture : TestBase<SquirrelReader>
    {
        [TestCaseSource(nameof(GetRootInstallers))]
        [Explicit]
        public void read_manifest(string path)
        {
            var comp = new CompressionService(logger);

            NugetSpec man;
            using (var zip = comp.TryOpen(path))
            {
                if (zip == null) Assert.Inconclusive("Couldn't open zip");
                man = Subject.GetNugetSpec(zip);
            }

            if (man == null)
            {

                Assert.Inconclusive("Not Squirrel");
            }
            man.Metadata.Title.Should().NotBeNullOrWhiteSpace();
            man.Metadata.Version.Should().NotBeNullOrWhiteSpace();
        }


        protected static string[] GetRootInstallers()
        {
            var files = Directory.GetFiles($"C:\\ProgramData\\AppGet\\Temp\\", "*.exe", SearchOption.AllDirectories);

            return files.ToArray();
        }
    }
}