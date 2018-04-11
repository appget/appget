using System;
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
        public  void get_builder()
        {
            var builder =  Subject.GetBuilder(new Uri("https://github.com/git-for-windows/git/releases/download/v2.16.2.windows.1/Git-2.16.2-32-bit.exe"));
            builder.Should().NotBeNull();
            builder.Version.Value.Should().Be("2.16.2");
            builder.Version.GetTop().Source.Should().Be("Url");
        }
    }
}