using AppGet.Manifests;
using SevenZip;

namespace AppGet.Installers.Msi
{
    public class MsiDetector : InstallerDetectorBase, IDetectInstallMethod
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

            return HasProperty(archive, InstallMethod.ToString()) ? 1 : 0;
        }
    }
}
