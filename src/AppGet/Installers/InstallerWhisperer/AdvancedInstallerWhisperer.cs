using AppGet.Manifest;

namespace AppGet.Installers.InstallerWhisperer
{
    public class AdvancedInstallerWhisperer : MsiWhisperer
    {
        public override InstallMethodTypes InstallMethod => InstallMethodTypes.AdvancedInstaller;

        public override string PassiveArgs => "/exebasicui /exelang 1033 /qb /norestart";
        public override string SilentArgs => "/exenoui /exelang 1033 /qn /norestart";

        public override string GetProcessPath()
        {
            return InstallerPath;
        }
    }
}