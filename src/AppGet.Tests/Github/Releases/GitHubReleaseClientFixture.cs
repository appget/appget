using AppGet.Github.Releases;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Github.Releases
{
    [TestFixture]
    public class GitHubReleaseClientFixture : TestBase<GitHubReleaseClient>
    {
        [Test]
        public  void get_releases()
        {
            WithRealHttp();

            var release =  Subject.GetReleases().Result;

            release.Should().NotBeEmpty();
            release[0].Version.Should().NotBeNull();
            release[0].Url.Should().EndWith(".exe");
        }
    }
}