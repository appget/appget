using System.Collections.Generic;
using System.Threading.Tasks;
using AppGet.Manifest;
using AppGet.Update;
using AppGet.Windows.WindowsInstaller;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Update
{
    [TestFixture]
    public class NovoClientFixture : TestBase<NovoClient>
    {
        private readonly IEnumerable<WindowsInstallerRecord> _installerRecords;

        [SetUp]
        public void Setup()
        {
            WithRealHttp();
        }

        public NovoClientFixture()
        {
            var installerClient = new WindowsInstallerClient();
            _installerRecords = installerClient.GetRecords();
        }

        [Test]
        public async Task should_get_updates()
        {
            var updates = await Subject.GetUpdates(_installerRecords);
            updates.Should().NotBeEmpty();
        }

        [Test]
        public async Task should_get_update()
        {
            var updates = await Subject.GetUpdate(_installerRecords, "python");
            updates.Should().NotBeNull();
        }

        [Test]
        public async Task should_return_empty_for_invalid_package()
        {
            var updates = await Subject.GetUpdate(_installerRecords, "not-valid");
            updates.Should().BeEmpty();
        }

        [Test]
        public async Task get_uninstall_records()
        {
            var updates = await Subject.GetUninstall(_installerRecords, "node-lts");
            updates.Should().NotBeEmpty();
            updates[0].InstallMethod.Should().Be(InstallMethodTypes.MSI);
            updates[0].PackageId.Should().Be("node-lts");
        }
    }
}