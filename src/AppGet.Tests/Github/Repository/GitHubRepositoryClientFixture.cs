using System.Threading.Tasks;
using AppGet.Github.Repository;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Github.Repository
{
    [TestFixture]
    public class GitHubRepositoryClientFixture : TestBase<GitHubRepositoryClient>
    {
        [TestCase("appget", "appget")]
        [TestCase("tim-lebedkov", "npackd-cpp")]
        public async Task should_get_repo(string owner, string name)
        {
            WithRealHttp();

            var repo = await Subject.Get(owner, name);
            repo.Should().NotBeNull();
        }
    }
}