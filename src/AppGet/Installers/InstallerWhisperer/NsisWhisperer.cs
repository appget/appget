using System.Collections.Generic;
using AppGet.Manifest;

namespace AppGet.Installers.InstallerWhisperer
{
    public class NsisWhisperer : InstallerBase
    {
        public override InstallMethodTypes InstallMethod => InstallMethodTypes.NSIS;

        public override string PassiveArgs => null;
        public override string SilentArgs => "/S";
        public override string LogArgs { get; }

        public override Dictionary<int, ExistReason> ExitCodes => new Dictionary<int, ExistReason>
        {
            {
                1, new ExistReason(ExitCodeTypes.UserCanceled)
            },
            {
                2, new ExistReason(ExitCodeTypes.Failed, "Installation aborted by script")
            }
        };
    }
}