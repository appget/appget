using AppGet.Manifests;
using SevenZip;

namespace AppGet.Installers.Msi
{
    public class MsiDetector : IDetectInstallMethod
    {
        public InstallMethodTypes InstallMethod => InstallMethodTypes.MSI;

        public decimal GetConfidence(string path, SevenZipExtractor archive)
        {
            if (path.ToLowerInvariant().EndsWith(".msi"))
            {
                return 1;
            }

            if (archive.ArchiveFileNames.Contains(".wixburn"))
            {
                return 1;
            }

            foreach (var prop in archive.ArchiveProperties)
            {
                if (prop.Value != null && prop.Value.ToString().ToUpperInvariant().Contains("MSI"))
                {
                    return 1;
                }
            }

            return 0;
        }
    }
}
