using AppGet.CreatePackage.Utils;
using NUnit.Framework;

namespace AppGet.Tests.CreatePackage.Utils
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
        [TestCase("https://github.com/", ExpectedResult = "")]
        [TestCase("http://google.com", ExpectedResult = null)]
        public string org_name(string url)
        {
            return new GithubUrl(url).OrganizationName;
        }


        [TestCase("https://github.com/AppGet/appget/releases", ExpectedResult = "https://github.com/AppGet/appget")]
        [TestCase("https://github.com/", ExpectedResult = "")]
        [TestCase("http://google.com", ExpectedResult = null)]
        public string repo_url(string url)
        {
            return new GithubUrl(url).RepositoryUrl;
        }
    }
}