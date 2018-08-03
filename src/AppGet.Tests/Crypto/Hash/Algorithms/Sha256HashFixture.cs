using AppGet.Manifest.Hash;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Crypto.Hash.Algorithms
{
    [TestFixture()]
    public class Sha256HashFixture : TestBase<Sha256>
    {
        [Test]
        public void should_get_sha1()
        {
            var path = this.GetType().Assembly.Location;
            var hash = Subject.CalculateHash(path);
            hash.Should().HaveLength(64);
        }
    }
}