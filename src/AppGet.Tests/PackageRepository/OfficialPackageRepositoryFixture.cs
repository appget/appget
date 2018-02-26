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

            var latest = await Subject.GetLatest(package);

            latest.Should().NotBeNull();
            latest.ManifestUrl.Should().StartWith("https://raw.githubusercontent.com/appget/packages/master/manifests/");
            latest.Id.Should().Be(package);
            //            latest.MajorVersion.Should().HaveLength(1);
        }

        [Test]
        public async Task should_get_null_for_unknown_id()
        {

            WithRealHttp();

            var latest = await Subject.GetLatest("bad-package-id");

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
            //            found.MajorVersion.Should().HaveLength(1);
        }

        [Test]
        public async Task should_get_manifest()
        {
            WithRealHttp();
            var c = await Subject.GetLatest("postman");

            c.Should().NotBeNull();
        }
    }
}