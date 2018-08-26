using System;
using System.Threading.Tasks;
using AppGet.CreatePackage;
using AppGet.Manifest;
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
            var builder = await Subject.GetBuilder(new Uri("https://update.pushbullet.com/pushbullet_installer.exe"));
            builder.Should().NotBeNull();
            builder.InstallMethod.Value.Should().Be(InstallMethodTypes.Inno);
        }
    }
}