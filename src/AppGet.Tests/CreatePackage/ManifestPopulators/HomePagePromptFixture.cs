using AppGet.CommandLine.Prompts;
using AppGet.CreatePackage;
using AppGet.CreatePackage.Root.Prompts;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.CreatePackage.ManifestPopulators
{
    [TestFixture]
    [Explicit]
    public class HomePagePromptFixture : TestBase<HomePagePrompt>
    {
        [SetUp]
        public void Setup()
        {
            Mocker.SetInstance<IUrlPrompt>(Mocker.Resolve<UrlPrompt>());
        }


        [Test]
        public void get_non_github_hostname_as_url()
        {
            var installer = new InstallerBuilder { Location = "https://microsoft.com/office" };
            var man = new PackageManifestBuilder();

            man.Installers.Add(installer);

            Subject.Invoke(man);

            man.Home.Top.Should().Be("https://microsoft.com");
        }

        [TestCase("https://download.microsoft.com/", ExpectedResult = "https://microsoft.com")]
        [TestCase("https://downloads.microsoft.com/", ExpectedResult = "https://microsoft.com")]
        [TestCase("https://update.microsoft.com/", ExpectedResult = "https://microsoft.com")]
        [TestCase("https://www.microsoft.com/", ExpectedResult = "https://www.microsoft.com")]
        [TestCase("https://updates.microsoft.com/", ExpectedResult = "https://microsoft.com")]
        [TestCase("https://swupdate.openvpn.org/community/", ExpectedResult = "https://openvpn.org")]
        [TestCase("https://cc.download.openvpn.org/community/", ExpectedResult = "https://openvpn.org")]
        [TestCase("https://a.openvpn.org/community/", ExpectedResult = "https://openvpn.org")]
        public string remove_update_download_segments(string url)
        {
            var manifestBuilder = new PackageManifestBuilder();
            manifestBuilder.Installers.Add( new InstallerBuilder { Location = url });

            Subject.Invoke(manifestBuilder);

            return manifestBuilder.Home.Top;
        }
    }
}