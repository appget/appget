using System.Threading.Tasks;
using AppGet.Manifests;
using NUnit.Framework;

namespace AppGet.Tests.Manifests
{
    [TestFixture]
    public class SubmissionClientFixture : TestBase<SubmissionClient>
    {
        [Test]
        public async Task submit()
        {
            WithRealHttp();
            await Subject.Submit(new PackageManifest
            {
                Id = "unit-test",
            }, "unittest.yaml");
        }

    }
}