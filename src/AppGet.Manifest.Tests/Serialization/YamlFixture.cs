using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Manifest.Tests.Serialization
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

            var yaml = manifest.ToYaml();

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

            var yaml = manifest.ToYaml();

            yaml.Should().Contain("args");
            yaml.Should().Contain("/S");
        }

        [Test]
        public void should_not_serialize_is_latest()
        {
            var manifest = new PackageManifest();

            var yaml = manifest.ToYaml();

            yaml.Should().NotContain("isLatest");
        }

        [Test]
        public void should_not_serialize_tag()
        {
            var manifest = new PackageManifest
            {
                Tag = "1.0"
            };

            var yaml = manifest.ToYaml();

            yaml.Should().NotContain("1.0");
        }
    }
}