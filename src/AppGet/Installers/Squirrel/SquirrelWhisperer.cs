using AppGet.HostSystem;
using AppGet.Manifest;
using AppGet.Windows;
using NLog;

namespace AppGet.Installers.Squirrel
{
    public class SquirrelWhisperer : InstallerWhispererBase
    {
        public SquirrelWhisperer(IProcessController processController, IPathResolver pathResolver, Logger logger)
            : base(processController, pathResolver, logger)
        {
        }

        public override InstallMethodTypes InstallMethod => InstallMethodTypes.Squirrel;

        protected override string InteractiveArgs => "";
        protected override string PassiveArgs => null;
        protected override string SilentArgs => "--silent";
    }
}