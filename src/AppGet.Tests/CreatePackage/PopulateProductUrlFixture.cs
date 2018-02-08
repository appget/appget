using System.Collections.Generic;
using AppGet.CreatePackage;
using AppGet.Manifests;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace AppGet.Tests.CreatePackage
{
    [TestFixture]
    public class PopulateProductUrlFixture : TestBase<PopulateProductUrl>
    {
        [Test]
        public void get_non_github_hostname_as_url()
        {
            var installer = new Installer { Location = "https://microsoft.com/office" };
            var man = new PackageManifest
            {
                Installers = new List<Installer> { installer }
            };

            Subject.Populate(man);

            man.ProductUrl.Should().Be("https://microsoft.com");
        }
    }
}