using System.Collections.Generic;
using AppGet.CommandLine.Prompts;
using AppGet.CreatePackage.ManifestPopulators;
using AppGet.Manifests;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace AppGet.Tests.CreatePackage.ManifestPopulators
{
    [TestFixture]
    public class PopulateProductUrlFixture : TestBase<PopulateProductUrl>
    {
        [Test]
        public void get_non_github_hostname_as_url()
        {
            Mocker.GetMock<IUrlPrompt>()
                .Setup(c => c.Request(It.IsAny<string>(), It.IsAny<string>()))
                .Returns<string, string>((message, defaultValue) => defaultValue);

            var installer = new Installer { Location = "https://microsoft.com/office" };
            var man = new PackageManifest
            {
                Installers = new List<Installer> { installer }
            };

            Subject.Populate(man, null);

            man.ProductUrl.Should().Be("https://microsoft.com");
        }
    }
}