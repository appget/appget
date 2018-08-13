using AppGet.Manifest;

namespace AppGet.Installers.InstallerWhisperer
{
    public class InstallBuilderWhisperer : InstallerBase
    {
        public override InstallMethodTypes InstallMethod => InstallMethodTypes.InstallBuilder;

        public override string PassiveArgs => "--mode unattended --unattendedmodeui minimal";
        public override string SilentArgs => "--mode unattended --unattendedmodeui none";

        public override string LogArgs { get; }
    }
}