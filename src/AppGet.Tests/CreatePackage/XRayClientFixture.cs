using System;
using System.Threading.Tasks;
using AppGet.CreatePackage;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.CreatePackage
{
    [TestFixture]
    public class XRayClientFixture : TestBase<XRayClient>
    {
        [SetUp]
        public void Setup()
        {
            WithRealHttp();
        }

        [Test]
        public async Task get_builder()
        {
            var builder = await Subject.GetBuilder(new Uri("https://github.com/git-for-windows/git/releases/download/v2.16.2.windows.1/Git-2.16.2-32-bit.exe"));
            builder.Should().NotBeNull();
            builder.Version.Value.Should().Be("2.16.2");
        }

        [Test]
        public async Task get_installer_builder()
        {
            var builder = await Subject.GetInstallerBuilder(new Uri("https://github.com/git-for-windows/git/releases/download/v2.16.2.windows.1/Git-2.16.2-32-bit.exe"));
            builder.Should().NotBeNull();
        }
    }
}