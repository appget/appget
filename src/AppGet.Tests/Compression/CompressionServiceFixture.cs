using System.Collections.Generic;
using System.IO;
using System.Linq;
using AppGet.Compression;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Compression
{

    [TestFixture(Category = "Local")]
    [Explicit]
    public class CompressionServiceFixture : TestBase<CompressionService>
    {

    }
}