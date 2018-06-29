using System.Collections.Generic;
using AppGet.HostSystem;
using AppGet.Manifest;
using AppGet.Processes;
using NLog;

namespace AppGet.Installers.Custom
{
    public class CustomWhisperer : InstallerWhispererBase
    {

        public override Dictionary<int, ExistReason> ExitCodes => new Dictionary<int, ExistReason>
        {
            // https://docs.microsoft.com/en-us/dotnet/framework/deployment/guide-for-administrators#return_codes
            {
                1602, new ExistReason(ExitCodeTypes.UserCanceled)
            },
            {
                1603, new ExistReason(ExitCodeTypes.Failed, "ERROR_INSTALL_FAILURE: A fatal error occurred during installation.")
            },
            {
                1641, new ExistReason(ExitCodeTypes.RestartRequired, "The installer has initiated a restart.", true)
            },
            {
                3010, new ExistReason(ExitCodeTypes.RestartRequired, null, true)
            },
            {
                5100, new ExistReason(ExitCodeTypes.RequirementUnmet, "The user's computer does not meet system requirements.")
            },
        };

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