using AppGet.Manifest;

namespace AppGet.Installers.InstallerWhisperer
{
    public class WixWhisperer : InstallerBase
    {
        public override InstallMethodTypes InstallMethod => InstallMethodTypes.Wix;

        // http://windows-installer-xml-wix-toolset.687559.n2.nabble.com/Running-Burn-driven-installer-in-quiet-mode-command-line-parameters-tp5913001p5913628.html
        public override string PassiveArgs => "/passive /norestart";
        public override string SilentArgs => "/quite /norestart";
        public override string LogArgs => "/l {path}";
    }
}