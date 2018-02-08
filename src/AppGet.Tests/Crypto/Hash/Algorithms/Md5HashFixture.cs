using AppGet.Crypto.Hash.Algorithms;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Crypto.Hash.Algorithms
{
    [TestFixture()]
    public class Md5HashFixture : TestBase<Md5Hash>
    {
        [Test]
        public void should_get_md5()
        {
            var path = this.GetType().Assembly.Location;
            var hash = Subject.CalculateHash(path);
            hash.Should().HaveLength(32);
        }
    }
}