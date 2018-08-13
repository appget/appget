using AppGet.Manifest;

namespace AppGet.Installers.UninstallerWhisperer
{
    public class WixUninstaller : UninstallerBase
    {
        public override InstallMethodTypes InstallMethod => InstallMethodTypes.Wix;
        public override string InteractiveArgs => "";

        // http://windows-installer-xml-wix-toolset.687559.n2.nabble.com/Running-Burn-driven-installer-in-quiet-mode-command-line-parameters-tp5913001p5913628.html
        public override string PassiveArgs => "/passive /norestart";
        public override string SilentArgs => "/quite /norestart";
        public override string LogArgs => "/l {path}";
    }
}