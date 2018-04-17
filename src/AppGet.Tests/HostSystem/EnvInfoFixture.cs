
using System.IO;
using AppGet.HostSystem;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.HostSystem
{
    [TestFixture]
    public class EnvInfoFixture : TestBase<EnvInfo>
    {
        [Test]
        public void should_get_os_info()
        {
            Subject.Name.Should().Contain("Windows");
            Subject.Version.Major.Should().BeGreaterThan(7);
            Subject.FullName.Should().Contain(Subject.Version.ToString());
            Subject.FullName.Should().Contain(Subject.Name);

            Subject.Is64BitOperatingSystem.Should().BeTrue();
            Subject.AppDir.Should().Contain(Path.DirectorySeparatorChar.ToString());
        }
    }
}