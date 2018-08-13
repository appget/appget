using AppGet.Manifest;

namespace AppGet.Installers.UninstallerWhisperer
{
    public class SquirrelUninstaller : UninstallerBase
    {
        public override InstallMethodTypes InstallMethod => InstallMethodTypes.Squirrel;

        public override string InteractiveArgs => "--uninstall";
        public override string PassiveArgs => "--uninstall";
        public override string SilentArgs => "--uninstall -s";
        public override string LogArgs => null;
    }
}