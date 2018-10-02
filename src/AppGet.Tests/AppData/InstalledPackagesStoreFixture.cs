using System;
using System.Linq;
using AppGet.AppData;
using AppGet.HostSystem;
using AppGet.Manifest;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.AppData
{
    [TestFixture]
    public class InstalledPackagesStoreFixture : TestBase<InstalledPackagesStore>
    {
        [SetUp]
        public void Setup()
        {
            Mocker.GetMock<IPathResolver>().SetupGet(c => c.AppDataDirectory).Returns(GetTestPath(Guid.NewGuid().ToString()));
            WithRealFileSystem();
        }

        [Test]
        public void no_file_should_return_empty_list()
        {
            Subject.Load().Should().HaveCount(0);
        }

        [Test]
        public void should_store_list()
        {
            Subject.Save(new PackageManifest { Id = "test-pkg", Version = "1.2.3" });


            var stored = Subject.Load().Single();

            stored.Id.Should().Be("test-pkg");
            stored.Version.Should().Be("1.2.3");
        }

        [Test]
        public void should_updat_existing()
        {
            Subject.Save(new PackageManifest { Id = "test-1", Version = "1.2.3" });
            Subject.Save(new PackageManifest { Id = "test-2", Version = "1.2.3" });
            Subject.Save(new PackageManifest { Id = "test-1", Version = "1.2.6" });


            var stored = Subject.Load();

            stored.Should().HaveCount(2);

            var test1 = stored.Single(c => c.Id == "test-1");

            test1.Version.Should().Be("1.2.6");
        }
    }
}