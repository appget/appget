using AppGet.Manifest;

namespace AppGet.Installers.InstallerWhisperer
{
    public class InstallShieldWhisperer : InstallerBase
    {
        public override InstallMethodTypes InstallMethod => InstallMethodTypes.InstallShield;

        public override string InteractiveArgs { get; }
        public override string PassiveArgs { get; }
        public override string SilentArgs { get; }
        public override string LogArgs { get; }
    }
}