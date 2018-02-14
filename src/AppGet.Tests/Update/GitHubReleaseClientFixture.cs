using AppGet.Update;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Update
{
    [TestFixture]
    public class GitHubReleaseClientFixture : TestBase<GitHubReleaseClient>
    {
        [Test]
        public void get_releases()
        {
            WithRealHttp();

            var release = Subject.GetReleases();

            release.Should().NotBeEmpty();
            release[0].Assets.Should().NotBeEmpty();
            release[0].Assets[0].browser_download_url.Should().EndWith(".exe");
            release[0].tag_name.Should().NotBeNullOrWhiteSpace();
        }
    }
}