using System.Linq;
using AppGet.InstalledPackages;
using AppGet.Manifests;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.InstalledPackages
{
    [TestFixture]
    public class WindowsInstallerInventoryManagerFixture : TestBase<WindowsInstallerInventoryManager>
    {
        [Test]
        public void should_get_uninstall_records()
        {
            var records = Subject.GetInstalledApplications();
            records.Should().NotBeEmpty();
        }

        [TestCase("Node JS")]
        [TestCase("VLC Media Player")]
        [TestCase("VLC-Media-Player")]
        public void should_find_install_record(string name)
        {
            Subject.GetInstalledApplications(name).Should().HaveCount(1);
        }
    }
}