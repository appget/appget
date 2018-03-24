using AppGet.Compression;
using AppGet.CreatePackage;
using AppGet.Installers.Wix;
using AppGet.Tools;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Installers.Wix
{
    public class WixDetectorFixture : DetectorTestBase<WixDetector>
    {
        [Test]
        [Explicit]
        public void harness()
        {
            const string path = @"C:\ProgramData\AppGet\Temp\ViberSetup.exe";
            var compression = Mocker.Resolve<CompressionService>();
            var manifestReader = Mocker.Resolve<PeManifestReader>();
            using (var zip = compression.TryOpen(path))
            {
                Subject.GetConfidence(path, zip, manifestReader.Read(path)).Should().NotBe(Confidence.None);
            }
        }
    }
}