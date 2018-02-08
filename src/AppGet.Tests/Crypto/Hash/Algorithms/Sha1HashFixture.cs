using AppGet.Crypto.Hash.Algorithms;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Crypto.Hash.Algorithms
{
    [TestFixture()]
    public class Sha1HashFixture : TestBase<Sha1Hash>
    {
        [Test]
        public void should_get_sha1()
        {
            var path = this.GetType().Assembly.Location;
            var hash = Subject.CalculateHash(path);
            hash.Should().HaveLength(40);
        }
    }
}