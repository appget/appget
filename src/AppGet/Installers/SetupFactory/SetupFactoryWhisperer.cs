using System.Collections.Generic;
using AppGet.HostSystem;
using AppGet.Manifest;
using AppGet.Processes;
using NLog;

namespace AppGet.Installers.SetupFactory
{
    public class SetupFactoryWhisperer : InstallerWhispererBase
    {
        // https://www.indigorose.com/webhelp/suf9/Program_Reference/Setup_Return_Codes.htm
        public override Dictionary<int, ExistReason> ExitCodes => new Dictionary<int, ExistReason>
        {
            {
                5, new ExistReason(ExitCodeTypes.UserCanceled)
            },
            {
                101, new ExistReason(ExitCodeTypes.RequirementUnmet, "The user's system did not satisfy the installation's system requirements.")
            },
            {
                103, new ExistReason(ExitCodeTypes.CorruptInstaller, "The installation's archive integrity check failed. This means that the setup's archive has been corrupted in some way.")
            },

        };

        public SetupFactoryWhisperer(IProcessController processController, IPathResolver pathResolver, Logger logger)
            : base(processController, pathResolver, logger)
        {
        }

        protected override InstallMethodTypes InstallMethod => InstallMethodTypes.SetupFactory;

        //Command line args: https://www.indigorose.com/webhelp/suf9/Program_Reference/Command_Line_Options.htm#Install_Command_Line_Options
        protected override string InteractiveArgs => "/W";
        protected override string PassiveArgs => null;
        protected override string SilentArgs => "/W /S";
    }
}