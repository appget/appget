using System.IO;
using AppGet.HostSystem;
using AppGet.Processes;
using AppGet.Tools;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Tools
{
    [TestFixture]
    public class SigCheckFixture : TestBase<SigCheck>
    {
        [Test]
        public void get_manifest()
        {
            Mocker.SetInstance<IEnvInfo>(new EnvInfo());
            Mocker.SetInstance<IProcessController>(new ProcessController(logger));

            var exe = Path.Combine(Mocker.Resolve<EnvInfo>().AppDir, "appget.exe");

            var manifest = Subject.GetManifest(exe);

            manifest.Should().NotBeNullOrWhiteSpace();
            manifest.Should().Contain("supportedOS");
            manifest.Should().Contain("assemblyIdentity");
            manifest.ToLowerInvariant().Should().NotContain(exe.ToLowerInvariant());
        }


        [Test]
        [Explicit]
        public void get_vlc()
        {
            Mocker.SetInstance<IEnvInfo>(new EnvInfo());
            Mocker.SetInstance<IProcessController>(new ProcessController(logger));


            var manifest = Subject.GetManifest("C:\\ProgramData\\AppGet\\Temp\\vlc-3.0.0-win32.exe");

            manifest.Should().NotBeNullOrWhiteSpace();
            manifest.Should().Contain("supportedOS");
            manifest.Should().Contain("assemblyIdentity");
        }
    }
}