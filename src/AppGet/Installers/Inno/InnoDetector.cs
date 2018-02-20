using System;
using AppGet.Manifests;
using SevenZip;

namespace AppGet.Installers.Inno
{
    public class InnoDetector : IDetectInstallMethod
    {
        public InstallMethodTypes InstallMethod => InstallMethodTypes.Inno;

        public decimal GetConfidence(string path, SevenZipExtractor archive)
        {
            foreach (var prop in archive.ArchiveProperties)
            {
                if (prop.Value != null && prop.Value.ToString().ToUpperInvariant().Contains("INNO"))
                {
                    return 1;
                }
            }

            return 0;
        }
    }
}
