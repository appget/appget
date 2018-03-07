using AppGet.CreatePackage;
using AppGet.Manifests;
using SevenZip;

namespace AppGet.Installers.Wix
{
    public class WixDetector : InstallerDetectorBase
    {
        public override InstallMethodTypes InstallMethod => InstallMethodTypes.Wix;

        public override Confidence GetConfidence(string path, SevenZipExtractor archive, string exeManifest)
        {
            if (archive != null && archive.ArchiveFileNames.Contains(".wixburn"))
            {
                return Confidence.Authoritative;
            }

            return base.GetConfidence(path, archive, exeManifest);
        }
    }
}
