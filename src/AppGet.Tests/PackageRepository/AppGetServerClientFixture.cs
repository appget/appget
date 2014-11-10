using System.Linq;
using AppGet.PackageRepository;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.PackageRepository
{
    [TestFixture]
    public class AppGetServerClientFixture : TestBase<AppGetServerClient>
    {
        [TestCase("linqpad")]
        [TestCase("seven-zip")]
        public void should_get_package(string package)
        {

            WithRealHttp();

            var latest = Subject.GetLatest(package);

            latest.Should().NotBeNull();
            latest.FlightPlanUrl.Should().StartWith("https://raw.githubusercontent.com/appget/appget.flightplans/master/flightplans/");
            latest.Id.Should().Be(package);
            latest.SourceUrl.Should().StartWith("https://github.com/appget/appget.flightplans/blob/master/flightplans");
            latest.MajorVersion.Should().HaveLength(1);
        }


        [TestCase("linq")]
        [TestCase("plus")]
        [TestCase("seven")]
        public void should_search_package(string term)
        {
            WithRealHttp();

            var found = Subject.Search(term).Single();

            found.Should().NotBeNull();
            found.FlightPlanUrl.Should().StartWith("https://raw.githubusercontent.com/appget/appget.flightplans/master/flightplans/");
            found.Id.Should().Contain(term);
            found.SourceUrl.Should().StartWith("https://github.com/appget/appget.flightplans/blob/master/flightplans");
            found.MajorVersion.Should().HaveLength(1);
        }
    }
}