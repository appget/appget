using System;
using AppGet.Manifest.Serialization;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Serialization
{
    [TestFixture]
    public class JsonFixture
    {
        [Test]
        public void correctly_serialize_version()
        {
            Json.Serialize(new Version("1.2.3")).Should().Be("\"1.2.3\"");
        }

        [Test]
        public void two_way_version()
        {
            var version = new Version("1.2.3.4");
            var json = Json.Serialize(version);
            Json.Deserialize<Version>(json).Should().Be(version);
        }
    }
}