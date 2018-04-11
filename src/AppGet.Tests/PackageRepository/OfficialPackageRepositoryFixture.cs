using System.Linq;
using AppGet.PackageRepository;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.PackageRepository
{
    [TestFixture]
    public class OfficialPackageRepositoryFixture : TestBase<OfficialPackageRepository>
    {
        [TestCase("vlc")]
        [TestCase("7-zip")]
        public  void should_get_package(string package)
        {

            WithRealHttp();

            var latest =  Subject.Get(package, null);

            latest.Should().NotBeNull();
            latest.ManifestPath.Should().StartWith("https://raw.githubusercontent.com/appget/packages/master/manifests/");
            latest.Id.Should().Be(package);
            latest.Tag.Should().BeNull();
        }

        [Test]
        public  void should_get_null_for_unknown_id()
        {

            WithRealHttp();

            var latest =  Subject.Get("bad-package-id", null);

            latest.Should().BeNull();
        }

        [Test]
        public  void should_get_null_for_unknown_tag()
        {

            WithRealHttp();

            var latest =  Subject.Get("vlc", "1");

            latest.Should().BeNull();
        }

        [TestCase("vlc")]
        [TestCase("plus")]
        [TestCase("zip")]
        public  void should_search_package(string term)
        {
            WithRealHttp();

            var results =  Subject.Search(term);

            var found = results.First();

            found.Should().NotBeNull();
            found.ManifestPath.Should().StartWith("https://raw.githubusercontent.com/appget/packages/master/manifests/");
            found.Id.Should().Contain(term);
        }

        [Test]
        public  void should_get_manifest()
        {
            WithRealHttp();
            var c =  Subject.Get("postman", null);

            c.Should().NotBeNull();
        }
    }
}