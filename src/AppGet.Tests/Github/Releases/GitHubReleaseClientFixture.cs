using System.Threading.Tasks;
using AppGet.Github.Releases;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Github.Releases
{
    [TestFixture]
    public class GitHubReleaseClientFixture : TestBase<GitHubReleaseClient>
    {
        [Test]
        public async Task get_releases()
        {
            WithRealHttp();

            var release = await Subject.GetLatest();

            release.Should().NotBeNull();
            release.Version.Should().NotBeNull();
            release.Url.Should().EndWith(".exe");
            release.Url.Should().Contain(release.Version.ToString());
        }
    }
}