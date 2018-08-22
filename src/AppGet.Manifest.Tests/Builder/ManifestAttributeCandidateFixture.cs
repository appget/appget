using AppGet.Manifest.Builder;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Manifest.Tests.Builder
{
    [TestFixture]
    public class ManifestAttributeCandidateFixture
    {
        [Test]
        public void should_use_string_as_source()
        {
            var attr = new ManifestAttributeCandidate<string>("Val", Confidence.Plausible, "MySource");

            attr.Value.Should().Be("Val");
            attr.Confidence.Should().Be(Confidence.Plausible);
            attr.Source.Should().Be("MySource");
        }

        [Test]
        public void should_use_object_as_source()
        {
            var attr = new ManifestAttributeCandidate<string>("Val", Confidence.Plausible, this);

            attr.Value.Should().Be("Val");
            attr.Confidence.Should().Be(Confidence.Plausible);
            attr.Source.Should().Be("ManifestAttributeCandidateFixture");
        }

        [Test]
        public void should_ignore_null_values()
        {
            var attr = new ManifestAttributeCandidate<string>(null, Confidence.Authoritative, this);

            attr.Value.Should().BeNull();
        }
    }
}