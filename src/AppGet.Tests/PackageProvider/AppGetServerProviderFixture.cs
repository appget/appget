using AppGet.Http;
using AppGet.PackageRepository;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.PackageProvider
{
    [TestFixture]
    public class AppGetServerProviderFixture : TestBase<AppGetServerClient>
    {
        [Test]
        public void should_get_flightplan()
        {
            Mocker.SetInstance<IHttpClient>(new HttpClient(logger));
            var c = Subject.GetLatest("linqpad");

            c.Should().NotBeNull();
        }
    }
}