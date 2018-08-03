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
    }
}