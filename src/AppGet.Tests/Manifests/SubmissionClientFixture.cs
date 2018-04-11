using AppGet.Manifests;
using NUnit.Framework;

namespace AppGet.Tests.Manifests
{
    [TestFixture]
    public class SubmissionClientFixture : TestBase<SubmissionClient>
    {
        [Test]
        public  void submit()
        {
            WithRealHttp();
             Subject.Submit(new PackageManifest
            {
                Id = "unit-test",
            }, "unittest.yaml");
        }

    }
}