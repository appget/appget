using AppGet.Manifests;
using AppGet.Serialization;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Serialization
{
    [TestFixture]
    public class YamlFixture
    {

        [Test]
        public void should_not_serialize_empty_args()
        {
            var manifest = new PackageManifest
            {
                Args = new InstallArgs(),
                Id = "test_manifest"
            };

            var yaml = Yaml.Serialize(manifest);

            yaml.Should().NotContain("args");
        }


        [Test]
        public void should_serialize_none_empty_empty_args()
        {
            var manifest = new PackageManifest
            {
                Args = new InstallArgs { Silent = "/S" },
                Id = "test_manifest"
            };

            var yaml = Yaml.Serialize(manifest);

            yaml.Should().Contain("args");
            yaml.Should().Contain("/S");
        }
    }
}