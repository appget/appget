using System.IO;
using AppGet.HostSystem;
using AppGet.Tools;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Tools
{
    [TestFixture]
    public class PeManifestReaderFixture : TestBase<PeManifestReader>
    {
        [Test]
        public void get_manifest()
        {
            var exe = Path.Combine(Mocker.Resolve<EnvInfo>().AppDir, "appget.exe");

            var manifest = Subject.Read(exe);

            manifest.Should().NotBeNullOrWhiteSpace();
            manifest.Should().Contain("supportedOS");
            manifest.Should().Contain("assemblyIdentity");
            manifest.ToLowerInvariant().Should().NotContain(exe.ToLowerInvariant());
        }


        [Test]
        [Explicit]
        public void get_vlc()
        {
            var manifest = Subject.Read(@"C:\Users\me\Downloads\dotPeek64.2017.3.3.exe");

            manifest.Should().NotBeNullOrWhiteSpace();
            manifest.Should().Contain("supportedOS");
            manifest.Should().Contain("assemblyIdentity");
        }
    }
}