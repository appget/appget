using AppGet.HostSystem;
using AppGet.Manifest;
using AppGet.Processes;
using NLog;

namespace AppGet.Installers.Squirrel
{
    public class SquirrelWhisperer : InstallerWhispererBase
    {
        public SquirrelWhisperer(IProcessController processController, IPathResolver pathResolver, Logger logger)
            : base(processController, pathResolver, logger)
        {
        }

        protected override InstallMethodTypes InstallMethod => InstallMethodTypes.Squirrel;

        protected override string InteractiveArgs => "";
        protected override string PassiveArgs => "";
        protected override string SilentArgs => "/S";
    }
}