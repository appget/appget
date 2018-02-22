using System.IO;
using AppGet.Crypto.Certificates;
using FluentAssertions;
using NUnit.Framework;

namespace AppGet.Tests.Crypto.Certificates
{
    [TestFixture()]
    public class FileSignatureServiceFixture: TestBase<FileSignatureService>
    {
        [Explicit]
        [TestCaseSource(nameof(GetRootInstallers))]
        public void match_root_installers(string name)
        {
            Subject.Get(name);
        }

        protected static string[] GetRootInstallers()
        {
            return Directory.GetFiles($"C:\\ProgramData\\AppGet\\Temp\\", "*.*", SearchOption.TopDirectoryOnly);
        }
    }
}