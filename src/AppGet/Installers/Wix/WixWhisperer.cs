using AppGet.HostSystem;
using AppGet.Manifest;
using AppGet.Windows;
using NLog;

namespace AppGet.Installers.Wix
{
    public class WixWhisperer : InstallerWhispererBase
    {
        public WixWhisperer(IProcessController processController, IPathResolver pathResolver, Logger logger)
            : base(processController, pathResolver, logger)
        {
        }

        public override InstallMethodTypes InstallMethod => InstallMethodTypes.Wix;

        // http://windows-installer-xml-wix-toolset.687559.n2.nabble.com/Running-Burn-driven-installer-in-quiet-mode-command-line-parameters-tp5913001p5913628.html
        protected override string InteractiveArgs => "";
        protected override string PassiveArgs => "/passive /norestart";
        protected override string SilentArgs => "/quite /norestart";

        protected override string LogArgs => "/l {path}";
    }
}