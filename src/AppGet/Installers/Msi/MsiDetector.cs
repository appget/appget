using AppGet.Manifests;
using SevenZip;

namespace AppGet.Installers.Msi
{
    public class MsiDetector : InstallerDetectorBase
    {
        public override InstallMethodTypes InstallMethod => InstallMethodTypes.MSI;

        public override decimal GetConfidence(string path, SevenZipExtractor archive, string exeManifest)
        {
            if (path.ToLowerInvariant().EndsWith(".msi"))
            {
                return 1;
            }

            if (archive != null && archive.ArchiveFileNames.Contains(".wixburn"))
            {
                return 1;
            }

            return base.GetConfidence(path, archive, exeManifest);
        }
    }
}
