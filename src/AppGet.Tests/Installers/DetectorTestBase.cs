using System.Collections.Generic;
using System.IO;
using System.Linq;
using AppGet.Compression;
using AppGet.CreatePackage;
using AppGet.HostSystem;
using AppGet.Installers;
using AppGet.Processes;
using AppGet.Tools;
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
            Mocker.SetInstance<IEnvInfo>(new EnvInfo());
            Mocker.SetInstance<IProcessController>(new ProcessController(logger));
        }


        [TestCaseSource(nameof(GetMatchingInstallers))]
        public void should_match_correct_installers(string path)
        {
            var compression = Mocker.Resolve<CompressionService>();
            var sigCheck = Mocker.Resolve<SigCheck>();
            using (var zip = compression.TryOpen(path))
            {
                Subject.GetConfidence(path, zip, sigCheck.GetManifest(path)).Should().NotBe(Confidence.NoMatch);
            }

        }


        [TestCaseSource(nameof(GetNonMatchingInstallers))]
        public void should_not_match_other_installers(string path)
        {
            var compression = Mocker.Resolve<CompressionService>();
            using (var zip = compression.TryOpen(path))
            {
                Subject.GetConfidence(path, zip, null).Should().Be(Confidence.NoMatch);
            }
        }

        [Explicit]
        [TestCaseSource(nameof(GetRootInstallers))]
        public void match_root_installers(string path)
        {
            var compression = Mocker.Resolve<CompressionService>();
            using (var zip = compression.TryOpen(path))
            {
                Subject.GetConfidence(path, zip, null).Should().NotBe(Confidence.NoMatch);
            }
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