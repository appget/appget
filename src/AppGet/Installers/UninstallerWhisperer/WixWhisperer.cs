using AppGet.Manifest;

namespace AppGet.Installers.UninstallerWhisperer
{
    public class WixUninstaller : UninstallerBase
    {
        public override InstallMethodTypes InstallMethod => InstallMethodTypes.Wix;
        public override string InteractiveArgs => "/uninstall";

        // http://windows-installer-xml-wix-toolset.687559.n2.nabble.com/Running-Burn-driven-installer-in-quiet-mode-command-line-parameters-tp5913001p5913628.html
        public override string PassiveArgs => $"{InteractiveArgs} /passive /norestart";
        public override string SilentArgs => $"{InteractiveArgs} /quite /norestart";
        public override string LogArgs => "/l {path}";
    }
}