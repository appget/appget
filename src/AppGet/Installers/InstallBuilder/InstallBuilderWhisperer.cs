using AppGet.HostSystem;
using AppGet.Manifests;
using AppGet.Processes;
using NLog;

namespace AppGet.Installers.InstallBuilder
{
    public class InstallBuilderWhisperer : InstallerWhispererBase
    {
        public InstallBuilderWhisperer(IProcessController processController, IPathResolver pathResolver, Logger logger)
            : base(processController, pathResolver, logger)
        {

        }

        protected override InstallMethodTypes InstallMethod => InstallMethodTypes.InstallBuilder;

        protected override bool HasLogs => false;

        protected override string InteractiveArgs => "";
        protected override string PassiveArgs => "--mode unattended --unattendedmodeui minimal";
        protected override string SilentArgs => "--mode unattended --unattendedmodeui none";
    }
}
