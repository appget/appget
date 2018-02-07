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
            latest.ManifestUrl.Should().StartWith("https://raw.githubusercontent.com/appget/packages/master/manifests/");
            latest.Id.Should().Be(package);
            latest.SourceUrl.Should().StartWith("https://github.com/appget/packages/blob/master/manifests/");
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
            found.ManifestUrl.Should().StartWith("https://raw.githubusercontent.com/appget/packages/master/manifests/");
            found.Id.Should().Contain(term);
            found.SourceUrl.Should().StartWith("https://github.com/appget/packages/blob/master/manifests/");
            found.MajorVersion.Should().HaveLength(1);
        }
    }
}