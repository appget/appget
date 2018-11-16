using AppGet.Manifest.Serialization;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Manifest.Tests.Serialization
{
    [TestFixture]
    public class JsonCloneTests
    {

        [Test]
        public void clone_should_create_new_instance()
        {
            var manifest = new PackageManifest { Name = "ABC", Id = "abc" };

            var clone = manifest.Clone();
            clone.Should().BeEquivalentTo(manifest);
            clone.Should().NotBe(manifest);
        }
    }
}