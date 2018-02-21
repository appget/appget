using AppGet.Manifests;
using SevenZip;

namespace AppGet.Installers.Nsis
{
    public class NsisDetector : InstallerDetectorBase, IDetectInstallMethod
    {
        public InstallMethodTypes InstallMethod => InstallMethodTypes.NSIS;


        public decimal GetConfidence(string path, SevenZipExtractor archive)
        {
            return HasProperty(archive, InstallMethod.ToString()) ? 1 : 0;
        }
    }
}
