using System.Linq;
using System.Threading.Tasks;
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
        public async Task should_get_package(string package)
        {

            WithRealHttp();

            var latest = await Subject.Get(package, null);

            latest.Should().NotBeNull();
            latest.ManifestUrl.Should().StartWith("https://raw.githubusercontent.com/appget/packages/master/manifests/");
            latest.Id.Should().Be(package);
            latest.Tag.Should().BeNull();
        }

        [Test]
        public async Task should_get_null_for_unknown_id()
        {

            WithRealHttp();

            var latest = await Subject.Get("bad-package-id", null);

            latest.Should().BeNull();
        }

        [Test]
        public async Task should_get_null_for_unknown_tag()
        {

            WithRealHttp();

            var latest = await Subject.Get("vlc", "1");

            latest.Should().BeNull();
        }


        [TestCase("vlc")]
        [TestCase("plus")]
        [TestCase("zip")]
        public async Task should_search_package(string term)
        {
            WithRealHttp();

            var results = await Subject.Search(term);

            var found = results.Single();

            found.Should().NotBeNull();
            found.ManifestUrl.Should().StartWith("https://raw.githubusercontent.com/appget/packages/master/manifests/");
            found.Id.Should().Contain(term);
        }

        [Test]
        public async Task should_get_manifest()
        {
            WithRealHttp();
            var c = await Subject.Get("postman", null);

            c.Should().NotBeNull();
        }
    }
}