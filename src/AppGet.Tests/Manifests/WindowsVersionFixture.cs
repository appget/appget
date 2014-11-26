using System;
using AppGet.Manifests;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Manifests
{
    public class WindowsVersionFixture
    {
        [TestCase("Windows XP", "5.1", 0)]
        [TestCase("Windows XP SP1", "5.1", 1)]
        [TestCase("Windows XP SP2", "5.1", 2)]
        [TestCase("Windows XP SP3", "5.1", 3)]
        [TestCase("Windows Vista", "6.0", 0)]
        [TestCase("Windows Vista SP1", "6.0", 1)]
        [TestCase("Windows Vista SP2", "6.0", 2)]
        [TestCase("Windows 7", "6.1", 0)]
        [TestCase("Windows 7 SP1", "6.1", 1)]
        [TestCase("Windows 8", "6.2", 0)]
        [TestCase("Windows 8.1", "6.3", 0)]
        public void should_parse_os(string input, string version, int servicePack)
        {
            var windowsVersion = WindowsVersion.Parse(input);

            windowsVersion.Version.ToString().Should().Be(version);
            windowsVersion.ServicePack.Should().Be(servicePack);
        }

        [TestCase("Windows xp")]
        [TestCase("windows XP")]
        [TestCase("Windows XP SP5")]
        public void should_throw_on_invalid_os(string input)
        {
            Assert.Throws<WindowsVersionParseException>(() => WindowsVersion.Parse(input));
        }

        [Test]
        public void should_get_current_version()
        {
            var currentOs = WindowsVersion.FromOperatingSystem(Environment.OSVersion);

            currentOs.Should().NotBeNull();
        }
    }
}