using AppGet.HostSystem;
using AppGet.Manifest;
using AppGet.Processes;
using NLog;

namespace AppGet.Installers.AdvancedInstaller
{
    public class AdvancedInstallerWhisperer : InstallerWhispererBase
    {
        public AdvancedInstallerWhisperer(IProcessController processController, IPathResolver pathResolver, Logger logger)
            : base(processController, pathResolver, logger)
        {
        }

        public override InstallMethodTypes InstallMethod => InstallMethodTypes.AdvancedInstaller;

        protected override string InteractiveArgs => "";
        protected override string PassiveArgs => "/exebasicui";
        protected override string SilentArgs => "/exenoui";
        protected override string LogArgs => "/exelog {path}";
    }
}