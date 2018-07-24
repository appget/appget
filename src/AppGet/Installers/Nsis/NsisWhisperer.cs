using AppGet.HostSystem;
using AppGet.Manifest;
using AppGet.Processes;
using NLog;

namespace AppGet.Installers.Nsis
{
    public class NsisWhisperer : InstallerWhispererBase
    {
        public NsisWhisperer(IProcessController processController, IPathResolver pathResolver, Logger logger)
            : base(processController, pathResolver, logger)
        {
        }

        public override InstallMethodTypes InstallMethod => InstallMethodTypes.NSIS;

        protected override string InteractiveArgs => "";
        protected override string PassiveArgs => null;
        protected override string SilentArgs => "/S";
    }
}