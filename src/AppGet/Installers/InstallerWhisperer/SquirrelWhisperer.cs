using AppGet.Manifest;

namespace AppGet.Installers.InstallerWhisperer
{
    public class SquirrelWhisperer : InstallerBase
    {
        public override InstallMethodTypes InstallMethod => InstallMethodTypes.Squirrel;

        public override string PassiveArgs => null;
        public override string SilentArgs => "--silent";
        public override string LogArgs => null;
    }
}