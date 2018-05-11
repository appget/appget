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
        [TestCase("7zip")]
        public void should_get_package(string package)
        {

            WithRealHttp();

            var latest = Subject.Get(package, null);

            latest.Should().NotBeNull();
            latest.ManifestPath.Should().StartWith("https://raw.githubusercontent.com/appget/packages/master/manifests/");
            latest.Id.Should().Be(package);
            latest.Tag.Should().Be("latest");
        }


        [TestCase("linqpad","5")]
        public void should_default_to_highest_tag(string package, string tag)
        {
            WithRealHttp();

            var latest = Subject.Get(package, null);

            latest.Should().NotBeNull();
            latest.ManifestPath.Should().StartWith("https://raw.githubusercontent.com/appget/packages/master/manifests/");
            latest.Id.Should().Be(package);
            latest.Tag.Should().Be(tag);
        }

        [Test]
        public void should_throw_unknown_id()
        {
            WithRealHttp();

            Assert.Throws<PackageNotFoundException>(() => Subject.Get("bad-package-id", null));
        }

        [Test]
        public void should_get_null_for_unknown_tag()
        {
            WithRealHttp();
            Assert.Throws<PackageNotFoundException>(() => Subject.Get("vlc", "1"));
        }

        [TestCase("vlc")]
        [TestCase("plus")]
        [TestCase("zip")]
        public void should_search_package(string term)
        {
            WithRealHttp();

            var results = Subject.Search(term);

            var found = results.First();

            found.Should().NotBeNull();
            found.ManifestPath.Should().StartWith("https://raw.githubusercontent.com/appget/packages/master/manifests/");
            found.Id.Should().Contain(term);
        }

        [Test]
        public void should_get_manifest()
        {
            WithRealHttp();
            var c = Subject.Get("postman", null);

            c.Should().NotBeNull();
        }
    }
}