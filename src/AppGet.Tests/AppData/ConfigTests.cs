using AppGet.AppData;
using AppGet.FileSystem;
using AppGet.HostSystem;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.AppData
{
    public class TestStore : StoreBase<Config>
    {
        protected override string Name => "test_config";

        public TestStore(IFileSystem fileSystem, IPathResolver pathResolver)
            : base(fileSystem, pathResolver)
        {
        }
    }

    [TestFixture]
    public class ConfigTests : TestBase<TestStore>
    {
        [Test]
        public void should_read_write_config()
        {
            Mocker.Use<IFileSystem>(new FileSystem.FileSystem(logger));
            Mocker.Use<IPathResolver>(new PathResolver());

            Subject.Save(c => c.ShareAnonymousData = false);
            Subject.Load().ShareAnonymousData.Should().Be(false);
            Subject.YamlText.Should().Contain("shareAnonymousData");
            
            Subject.Save(c => c.ShareAnonymousData = true);
            Subject.Load().ShareAnonymousData.Should().Be(true);
            Subject.YamlText.Should().NotContain("shareAnonymousData");

        }
    }
}