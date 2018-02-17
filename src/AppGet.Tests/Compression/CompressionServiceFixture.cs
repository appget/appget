using System.Collections.Generic;
using System.IO;
using System.Linq;
using AppGet.Compression;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Compression
{

    [TestFixture(Category = "Local")]
    public class CompressionServiceFixture : TestBase<CompressionService>
    {
        [TestCaseSource(nameof(GetInstallers), Category = "Local")]
        public void try_open(string name)
        {
            var zip = Subject.TryOpen(Path.Combine("C:\\ProgramData\\AppGet\\Temp\\", name));
            zip.Should().NotBeNull();
        }


        private static List<string> GetInstallers()
        {
            return Directory.GetFiles("C:\\ProgramData\\AppGet\\Temp\\Inno\\", "*.*")
                .Concat(Directory.GetFiles("C:\\ProgramData\\AppGet\\Temp\\NSIS\\", "*.*"))
                .Concat(Directory.GetFiles("C:\\ProgramData\\AppGet\\Temp\\MSI\\", "*.*"))
                .Concat(Directory.GetFiles("C:\\ProgramData\\AppGet\\Temp\\Squirrel\\", "*.*"))
                .Concat(Directory.GetFiles("C:\\ProgramData\\AppGet\\Temp\\", "*.*")).ToList();
        }
    }
}