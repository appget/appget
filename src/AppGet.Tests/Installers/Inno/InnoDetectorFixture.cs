using System.Collections.Generic;
using System.IO;
using System.Linq;
using AppGet.Compression;
using AppGet.Installers.Inno;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Installers.Inno
{
    [TestFixture(Category = "Local")]
    public class InnoDetectorFixture : TestBase<InnoDetector>
    {
        [TestCaseSource(nameof(GetMatchingInstallers))]
        public void should_flag_inno(string name)
        {
            var compression = Mocker.Resolve<CompressionService>();
            Subject.GetConfidence(name, compression.TryOpen(name)).Should().Be(1);
        }


        [TestCaseSource(nameof(GetNon_Matching_installers))]
        public void should_not_flag_non_inno(string name)
        {
            var compression = Mocker.Resolve<CompressionService>();
            Subject.GetConfidence(name, compression.TryOpen(name)).Should().Be(0);
        }



        private static string[] GetMatchingInstallers()
        {
            return Directory.GetFiles("C:\\ProgramData\\AppGet\\Temp\\Inno\\", "*.*");
        }

        private static List<string> GetNon_Matching_installers()
        {
            return Directory.GetFiles("C:\\ProgramData\\AppGet\\Temp\\NSIS\\", "*.*")
                .Concat(Directory.GetFiles("C:\\ProgramData\\AppGet\\Temp\\MSI\\", "*.*"))
                .Concat(Directory.GetFiles("C:\\ProgramData\\AppGet\\Temp\\Squirrel\\", "*.*"))
                .Concat(Directory.GetFiles("C:\\ProgramData\\AppGet\\Temp\\", "*.*")).ToList();
        }
    }
}