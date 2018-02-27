using AppGet.Manifests;

namespace AppGet.Installers.Nsis
{
    public class NsisDetector : InstallerDetectorBase
    {
        public override InstallMethodTypes InstallMethod => InstallMethodTypes.NSIS;
    }
}
