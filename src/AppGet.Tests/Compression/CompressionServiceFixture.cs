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
        [TestCaseSource(nameof(GetInstallers), Category = "Local")]
        public void try_open(string name)
        {
            using (var zip = Subject.TryOpen(Path.Combine("C:\\ProgramData\\AppGet\\Temp\\", name)))
            {
                Assert.Inconclusive(zip.Format.ToString());
                zip.Should().NotBeNull();
            }
        }

        //        [Test]
        //        public void open()
        //        {
        //            using (var zip = Subject.TryOpen(@"C:\ProgramData\AppGet\Temp\android-studio-ide-171.4443003-windows.exe"))
        //            {
        //                zip.Should().NotBeNull();
        //            }
        //        }


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