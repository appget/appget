using System.Collections.Generic;
using AppGet.Manifest;

namespace AppGet.Installers.InstallerWhisperer
{
    public class CustomWhisperer : InstallerBase
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


        public override InstallMethodTypes InstallMethod => InstallMethodTypes.Custom;

        public override string PassiveArgs => null;
        public override string SilentArgs => null;
        public override string LogArgs => null;
    }
}