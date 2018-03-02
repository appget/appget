using System.Threading.Tasks;
using AppGet.Github.Releases;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Update
{
    [TestFixture]
    public class GitHubReleaseClientFixture : TestBase<GitHubReleaseClient>
    {
        [Test]
        public async Task get_releases()
        {
            WithRealHttp();

            var release = await Subject.GetReleases();

            release.Should().NotBeEmpty();
            release[0].Version.Should().NotBeNull();
            release[0].Url.Should().EndWith(".exe");
        }
    }
}