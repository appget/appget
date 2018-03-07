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
            var sigCheck = Mocker.Resolve<SigCheck>();
            using (var zip = compression.TryOpen(path))
            {
                Subject.GetConfidence(path, zip, sigCheck.GetManifest(path)).Should().NotBe(Confidence.None);
            }
        }
    }
}