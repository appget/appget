using System.Collections.Generic;
using System.IO;
using System.Linq;
using AppGet.Compression;
using AppGet.Installers;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Installers
{
    [TestFixture(Category = "Local")]
    public abstract class DetectorTestBase<T> : TestBase<T> where T : class, IDetectInstallMethod
    {
        [SetUp]
        public void Setup()
        {

        }

        [TestCaseSource(nameof(GetMatchingInstallers))]
        public void should_match_correct_installers(string name)
        {
            var compression = Mocker.Resolve<CompressionService>();
            Subject.GetConfidence(name, compression.TryOpen(name)).Should().Be(1);
        }


        [TestCaseSource(nameof(GetNonMatchingInstallers))]
        public void should_not_match_other_installers(string name)
        {
            var compression = Mocker.Resolve<CompressionService>();
            Subject.GetConfidence(name, compression.TryOpen(name)).Should().Be(0);
        }

        [Explicit]
        [TestCaseSource(nameof(GetRootInstallers))]
        public void match_root_installers(string name)
        {
            var compression = Mocker.Resolve<CompressionService>();
            Subject.GetConfidence(name, compression.TryOpen(name)).Should().Be(1);
        }

        private static string Type()
        {
            return typeof(T).Namespace?.Split('.').Last();
        }


        protected static string[] GetRootInstallers()
        {
            var files = Directory.GetFiles($"C:\\ProgramData\\AppGet\\Temp\\", "*.*", SearchOption.TopDirectoryOnly);

            var categorizedInstallers = GetMatchingInstallers().Concat(GetNonMatchingInstallers()).Select(Path.GetFileName);

            return files.Where(c => categorizedInstallers.All(d => Path.GetFileName(c) != d)).ToArray();
        }

        protected static string[] GetMatchingInstallers()
        {
            return Directory.GetFiles($"C:\\ProgramData\\AppGet\\Temp\\{Type()}\\");
        }

        protected static List<string> GetNonMatchingInstallers()
        {
            var dirs = Directory.GetDirectories("C:\\ProgramData\\AppGet\\Temp\\")
                .Where(c => !c.ToLowerInvariant().Contains(Type().ToLowerInvariant()));
            return dirs.SelectMany(Directory.GetFiles).ToList();
        }
    }
}