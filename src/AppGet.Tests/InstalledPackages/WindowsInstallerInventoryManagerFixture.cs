using AppGet.InstalledPackages;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.InstalledPackages
{
    public class WindowsInstallerInventoryManagerFixture : TestBase<WindowsInstallerInventoryManager>
    {
        [Test]
        public void should_get_uninstall_records()
        {
            var records = Subject.GetInstalledApplications();
            records.Should().NotBeEmpty();
        }

        [TestCase("Node JS", Category = "Local")]
        [TestCase("VLC Media Player", Category = "Local")]
        [TestCase("VLC-Media-Player", Category = "Local")]
        public void should_find_install_record(string name)
        {
            Subject.GetInstalledApplications(name).Should().HaveCount(1);
        }
    }
}