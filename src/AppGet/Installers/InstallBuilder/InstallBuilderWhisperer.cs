using AppGet.HostSystem;
using AppGet.Manifest;
using AppGet.Windows;
using NLog;

namespace AppGet.Installers.InstallBuilder
{
    public class InstallBuilderWhisperer : InstallerWhispererBase
    {
        public InstallBuilderWhisperer(IProcessController processController, IPathResolver pathResolver, Logger logger)
            : base(processController, pathResolver, logger)
        {
        }

        public override InstallMethodTypes InstallMethod => InstallMethodTypes.InstallBuilder;

        protected override string InteractiveArgs => "";
        protected override string PassiveArgs => "--mode unattended --unattendedmodeui minimal";
        protected override string SilentArgs => "--mode unattended --unattendedmodeui none";
    }
}