using AppGet.Manifest;

namespace AppGet.Installers.InstallerWhisperer
{
    public class AdvancedInstallerWhisperer : MsiWhisperer
    {
        public override InstallMethodTypes InstallMethod => InstallMethodTypes.AdvancedInstaller;

        public override string PassiveArgs => $"/exebasicui {base.PassiveArgs}";
        public override string SilentArgs => $"/exenoui {base.SilentArgs}";

        public override string GetProcessPath()
        {
            return InstallerPath;
        }
    }
}