using AppGet.Manifest;

namespace AppGet.Installers.UninstallerWhisperer
{
    public class NsisUninstaller : UninstallerBase
    {
        public override InstallMethodTypes InstallMethod => InstallMethodTypes.NSIS;
        public override string InteractiveArgs => "";

        public override string PassiveArgs => null;
        public override string SilentArgs => "/S";
        public override string LogArgs { get; }
    }
}