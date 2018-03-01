using AppGet.CreatePackage.Parsers;
using NUnit.Framework;

namespace AppGet.Tests.CreatePackage.Parsers
{

    [TestFixture]
    public class GithubUrlFixture
    {
        [TestCase("https://github.com/AppGet/appget/releases", ExpectedResult = true)]
        [TestCase("http://google.com", ExpectedResult = false)]
        public bool is_valid(string url)
        {
            return new GithubUrl(url).IsValid;
        }

        [TestCase("https://github.com/AppGet/appget/releases", ExpectedResult = "AppGet")]
        [TestCase(" https://github.com/tim-lebedkov/npackd-cpp/releases/download/version_1.23.2/NpackdCL-1.23.2.msi", ExpectedResult = "tim-lebedkov")]
        [TestCase("https://github.com/AppGet/", ExpectedResult = "AppGet")]
        [TestCase("https://github.com/", ExpectedResult = null)]
        [TestCase("http://google.com", ExpectedResult = null)]
        public string owner(string url)
        {
            return new GithubUrl(url).Owner;
        }


        [TestCase("https://github.com/AppGet/appget/releases", ExpectedResult = "appget")]
        [TestCase("https://github.com/AppGet/appget.packages/releases", ExpectedResult = "appget.packages")]
        [TestCase(" https://github.com/tim-lebedkov/npackd-cpp/releases/download/version_1.23.2/NpackdCL-1.23.2.msi", ExpectedResult = "npackd-cpp")]

        [TestCase("https://github.com/AppGet/", ExpectedResult = null)]
        [TestCase("https://github.com/", ExpectedResult = null)]
        [TestCase("http://google.com", ExpectedResult = null)]
        public string repo(string url)
        {
            return new GithubUrl(url).Repository;
        }


        [TestCase("https://github.com/AppGet/appget/releases", ExpectedResult = "https://github.com/AppGet/appget")]
        [TestCase("https://github.com/", ExpectedResult = null)]
        [TestCase("http://google.com", ExpectedResult = null)]
        public string repo_url(string url)
        {
            return new GithubUrl(url).RepositoryUrl;
        }
    }
}