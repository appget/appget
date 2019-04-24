using AppGet.Manifest.Hash;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Manifest.Tests.Hash
{
    [TestFixture]
    public class Sha256Tests
    {
        [Test]
        public void should_get_sha1()
        {
            var path = GetType().Assembly.Location;
            var hash = new Sha256().CalculateHash(path);
            hash.Should().HaveLength(64);
        }
    }
}