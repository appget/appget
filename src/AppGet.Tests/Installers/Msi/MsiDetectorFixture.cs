using System.Collections.Generic;
using System.IO;
using System.Linq;
using AppGet.Compression;
using AppGet.Installers.Msi;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Installers.Msi
{
    [TestFixture(Category = "Local")]
    public class MsiDetectorFixture : TestBase<MsiDetector>
    {
        [TestCaseSource(nameof(GetMSIInstallers))]
        public void should_flag_msi(string name)
        {
            var compression = Mocker.Resolve<CompressionService>();
            Subject.GetConfidence(name, compression.TryOpen(name)).Should().Be(1);
        }


        [TestCaseSource(nameof(GetNon_MSI_Installers))]
        public void should_not_flag_non_msi(string name)
        {
            var compression = Mocker.Resolve<CompressionService>();
            Subject.GetConfidence(name, compression.TryOpen(name)).Should().Be(0);
        }



        private static string[] GetMSIInstallers()
        {
            return Directory.GetFiles("C:\\ProgramData\\AppGet\\Temp\\MSI\\", "*.*");
        }

        private static List<string> GetNon_MSI_Installers()
        {
            return Directory.GetFiles("C:\\ProgramData\\AppGet\\Temp\\NSIS\\", "*.*")
                .Concat(Directory.GetFiles("C:\\ProgramData\\AppGet\\Temp\\Inno\\", "*.*"))
                .Concat(Directory.GetFiles("C:\\ProgramData\\AppGet\\Temp\\Squirrel\\", "*.*"))
                .Concat(Directory.GetFiles("C:\\ProgramData\\AppGet\\Temp\\", "*.*")).ToList();
        }
    }
}