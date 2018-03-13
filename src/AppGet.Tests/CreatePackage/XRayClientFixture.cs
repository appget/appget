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
            var builder = await Subject.GetBuilder(new Uri("https://dl.google.com/dl/android/studio/install/3.0.1.0/android-studio-ide-171.4443003-windows.exe"));
            builder.Should().NotBeNull();
            builder.Version.Value.Should().Be("3.0.1.0");
            builder.Version.GetTop().Source.Should().Be("Url");
        }
    }
}