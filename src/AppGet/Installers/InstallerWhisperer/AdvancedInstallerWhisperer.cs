using AppGet.Manifest;

namespace AppGet.Installers.InstallerWhisperer
{
    public class AdvancedInstallerWhisperer : InstallerBase
    {
        public override InstallMethodTypes InstallMethod => InstallMethodTypes.AdvancedInstaller;

        public override string PassiveArgs => "/exebasicui";
        public override string SilentArgs => "/exenoui";
        public override string LogArgs => "/exelog {path}";
    }
}