using System.IO;
using System.Linq;
using AppGet.Windows;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Windows
{
    [TestFixture]
    public class FileLockFinderFixture
    {
        [Test]
        public void get_locks()
        {
            var files = Directory.GetFiles("C:\\Program Files\\Viscosity\\", "*.exe", SearchOption.AllDirectories);

            var p = FileLockFinder.WhoIsLocking(files.Select(c => c.ToUpper()).ToArray()).ToList();

            p.Should().NotBeEmpty();
        }
    }
}