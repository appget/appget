using System.Threading.Tasks;
using AppGet.Installers;
using NUnit.Framework;

namespace AppGet.Tests
{
    [TestFixture]
    public class NexClientFixture : TestBase<NexClient>
    {
        [Test]
        public async Task submit_report()
        {
            WithRealHttp();
            await Subject.SubmitReport(new InstallerReport());
        }
    }
}