using System.Collections.Generic;
using AppGet.CommandLine.Prompts;
using AppGet.CreatePackage.ManifestPopulators;
using AppGet.Manifests;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.CreatePackage.ManifestPopulators
{
    [TestFixture]
    public class PopulateProductUrlFixture : TestBase<PopulateProductUrl>
    {
        [SetUp]
        public void Setup()
        {
            Mocker.SetInstance<IUrlPrompt>(Mocker.Resolve<UrlPrompt>());
        }


        [Test]
        public void get_non_github_hostname_as_url()
        {
            var installer = new Installer { Location = "https://microsoft.com/office" };
            var man = new PackageManifest
            {
                Installers = new List<Installer> { installer }
            };

            Subject.Populate(man, null, false);

            man.ProductUrl.Should().Be("https://microsoft.com");
        }

        [TestCase("https://download.microsoft.com/", ExpectedResult = "https://microsoft.com")]
        [TestCase("https://downloads.microsoft.com/", ExpectedResult = "https://microsoft.com")]
        [TestCase("https://update.microsoft.com/", ExpectedResult = "https://microsoft.com")]
        [TestCase("https://updates.microsoft.com/", ExpectedResult = "https://microsoft.com")]
        public string remove_update_download_segments(string url)
        {
            var installer = new Installer { Location = url };
            var man = new PackageManifest
            {
                Installers = new List<Installer> { installer }
            };

            Subject.Populate(man, null, false);

            return man.ProductUrl;
        }
    }
}