using AppGet.Manifest;

namespace AppGet.Installers.InstallerWhisperer
{
    public class NsisWhisperer : InstallerBase
    {
        public override InstallMethodTypes InstallMethod => InstallMethodTypes.NSIS;

        public override string PassiveArgs => null;
        public override string SilentArgs => "/S";
        public override string LogArgs { get; }
    }
}