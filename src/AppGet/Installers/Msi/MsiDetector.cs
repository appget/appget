using AppGet.CreatePackage;
using AppGet.Manifests;
using SevenZip;

namespace AppGet.Installers.Msi
{
    public class MsiDetector : InstallerDetectorBase
    {
        public override InstallMethodTypes InstallMethod => InstallMethodTypes.MSI;

        public override Confidence GetConfidence(string path, SevenZipExtractor archive, string exeManifest)
        {
            if (path.ToLowerInvariant().EndsWith(".msi"))
            {
                return Confidence.Authoritative;
            }

            if (archive != null && archive.ArchiveFileNames.Contains(".wixburn"))
            {
                return Confidence.Plausible;
            }

            return base.GetConfidence(path, archive, exeManifest);
        }
    }
}
