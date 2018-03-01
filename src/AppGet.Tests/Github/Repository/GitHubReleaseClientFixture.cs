using System.Threading.Tasks;
using AppGet.Github.Repository;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Github.Repository
{
    [TestFixture]
    public class GitHubReleaseClientFixture : TestBase<GitHubRepositoryClient>
    {
        [Test]
        public async Task should_get_repo()
        {
            WithRealHttp();

            var repo = await Subject.Get("appget", "appget");
            repo.Should().NotBeNull();
            repo.homepage.Should().Be("https://appget.net");
        }
    }
}