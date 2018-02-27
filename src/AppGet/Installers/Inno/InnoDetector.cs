using AppGet.Manifests;

namespace AppGet.Installers.Inno
{
    public class InnoDetector : InstallerDetectorBase
    {
        public override InstallMethodTypes InstallMethod => InstallMethodTypes.Inno;
    }
}
