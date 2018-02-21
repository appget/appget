using AppGet.Manifests;
using SevenZip;

namespace AppGet.Installers.Inno
{
    public class InnoDetector : InstallerDetectorBase, IDetectInstallMethod
    {
        public InstallMethodTypes InstallMethod => InstallMethodTypes.Inno;

        public decimal GetConfidence(string path, SevenZipExtractor archive)
        {
            return HasProperty(archive, InstallMethod.ToString()) ? 1 : 0;
        }
    }
}
