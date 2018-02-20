using AppGet.Manifests;
using SevenZip;

namespace AppGet.Installers.Nsis
{
    public class NsisDetector : IDetectInstallMethod
    {
        public InstallMethodTypes InstallMethod => InstallMethodTypes.NSIS;


        public decimal GetConfidence(string path, SevenZipExtractor archive)
        {
            foreach (var prop in archive.ArchiveProperties)
            {
                if (prop.Value != null && prop.Value.ToString().ToUpperInvariant().Contains("NSIS"))
                {
                    return 1;
                }
            }

            return 0;
        }
    }
}
