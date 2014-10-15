using System.Linq;
using AppGet.FlightPlans;
using AppGet.InstalledPackages;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.InstalledPackages
{
    [TestFixture]
    public class WindowsInstallerInventoryManagerFixture : TestBase<WindowsInstallerInventoryManager>
    {
        [Test]
        public void should_get_uninstall_recoreds()
        {
            var records = Subject.GetInstalledApplication();

            records.Should().NotBeEmpty();

            var nonMsi = records.Where(c => c.InstallMethod == InstallMethodType.Zip).ToList();
        }
    }
}