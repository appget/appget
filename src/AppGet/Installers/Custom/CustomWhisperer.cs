using AppGet.HostSystem;
using AppGet.Manifests;
using AppGet.Processes;
using NLog;

namespace AppGet.Installers.Custom
{
    public class CustomWhisperer : InstallerWhispererBase
    {
        public CustomWhisperer(IProcessController processController, IPathResolver pathResolver, Logger logger)
            : base(processController, pathResolver, logger)
        {

        }

        protected override InstallMethodTypes InstallMethod => InstallMethodTypes.Custom;

        protected override string InteractiveArgs => null;
        protected override string PassiveArgs => null;
        protected override string SilentArgs => null;
        protected override string LogArgs => null;
    }
}
