using System.Threading.Tasks;
using AppGet.Update;
using AppGet.Windows.WindowsInstaller;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Update
{
    [TestFixture]
    public class NovoClientFixture : TestBase<NovoClient>
    {
        [Test]
        public async Task should_get_updates()
        {
            WithRealHttp();

            var installerClient = new WindowsInstallerClient();

            var records = installerClient.GetRecords();

            var updates = await Subject.GetUpdates(records);

            updates.Should().NotBeEmpty();
        }

        [Test]
        public async Task should_get_update()
        {
            WithRealHttp();

            var installerClient = new WindowsInstallerClient();

            var records = installerClient.GetRecords();

            var updates = await Subject.GetUpdate(records, "python");

            updates.Should().NotBeNull();
        }

        [Test]
        public async Task should_return_empty_for_invalid_package()
        {
            WithRealHttp();

            var installerClient = new WindowsInstallerClient();

            var records = installerClient.GetRecords();

            var updates = await Subject.GetUpdate(records, "not-valid");
            updates.Should().BeEmpty();
        }
    }
}