using AppGet.Http;
using AppGet.PackageProvider;
using NUnit.Framework;

namespace AppGet.Tests.PackageProvider
{
    [TestFixture]
    public class AppGetServerProviderFixture : TestBase<AppGetServerProvider>
    {
        [Test]
        public void should_get_flightplan()
        {

            Mocker.SetInstance<IHttpClient>(new HttpClient(logger));

            var c = Subject.GetFlightPlan("linqpad");
        }
    }
}