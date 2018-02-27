using System;
using System.IO;
using System.Linq;
using AppGet.Installers.Inno;
using Microsoft.Practices.ObjectBuilder2;
using NUnit.Framework;

namespace AppGet.Tests.Installers.Inno
{
    public class InnoDetectorFixture : DetectorTestBase<InnoDetector>
    {

        [Test]
        public void clean_known_from_root()
        {
            var knownDirs = Directory.GetDirectories("C:\\ProgramData\\AppGet\\Temp\\");

            var known = knownDirs.SelectMany(c => Directory.GetFiles(c).Select(Path.GetFileName)).ToList();
            var unknow = GetRootInstallers().Select(Path.GetFileName).ToList();


            var d = known.Intersect(unknow);

            known.ForEach(Console.WriteLine);
        }
    }
}