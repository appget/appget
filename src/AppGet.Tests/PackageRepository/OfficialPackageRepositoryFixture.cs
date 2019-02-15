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
        [TestCase("7zip")]
        public async Task should_get_package(string package)
        {
            WithRealHttp();

            var latest = await Subject.GetAsync(package, null);

            latest.Should().NotBeNull();
            latest.ManifestPath.Should().StartWith("https://raw.githubusercontent.com/appget/appget.packages/master/manifests/");
            latest.Id.Should().Be(package);
            latest.Tag.Should().BeNull();
            latest.IsLatest.Should().BeTrue();
        }


        [TestCase("linqpad", "5")]
        public async Task should_default_to_highest_tag(string package, string tag)
        {
            WithRealHttp();

            var latest = await Subject.GetAsync(package, null);

            latest.Should().NotBeNull();
            latest.ManifestPath.Should().StartWith("https://raw.githubusercontent.com/appget/appget.packages/master/manifests/");
            latest.Id.Should().Be(package);
            latest.Tag.Should().Be(tag);
        }

        [Test]
        public void should_throw_unknown_id()
        {
            WithRealHttp();

            Assert.ThrowsAsync<PackageNotFoundException>(() => Subject.GetAsync("bad-package-id", null));
        }

        [Test]
        public void should_get_null_for_unknown_tag()
        {
            WithRealHttp();
            Assert.ThrowsAsync<PackageNotFoundException>(() => Subject.GetAsync("vlc", "1"));
        }

        [TestCase("vlc")]
        [TestCase("plus")]
        [TestCase("zip")]
        public async Task should_search_package(string term)
        {
            WithRealHttp();

            var results = await Subject.Search(term);

            var found = results.First();

            found.Should().NotBeNull();
            found.ManifestPath.Should().StartWith("https://raw.githubusercontent.com/appget/appget.packages/master/manifests/");
            found.Id.Should().Contain(term);
        }

        [Test]
        public void should_get_manifest()
        {
            WithRealHttp();
            var c = Subject.GetAsync("postman", null);

            c.Should().NotBeNull();
        }
    }
}