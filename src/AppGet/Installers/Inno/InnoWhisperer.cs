using System.Collections.Generic;
using AppGet.HostSystem;
using AppGet.Manifest;
using AppGet.Processes;
using NLog;

namespace AppGet.Installers.Inno
{
    public class InnoWhisperer : InstallerWhispererBase
    {
        // http://www.jrsoftware.org/ishelp/index.php?topic=setupexitcodes
        public override Dictionary<int, ExistReason> ExitCodes => new Dictionary<int, ExistReason>
        {
            {
                1, new ExistReason(ExitCodeTypes.Failed, "Setup failed to initialize")
            },
            {
                2, new ExistReason(ExitCodeTypes.UserCanceled)
            },
            {
                3,
                new ExistReason(ExitCodeTypes.Failed,
                    "A fatal error occurred while preparing to move to the next installation phase. This should never happen except under the most unusual of circumstances, such as running out of memory or Windows resources.")
            },
            {
                4, new ExistReason(ExitCodeTypes.Failed, "A fatal error occurred during the actual installation process.")
            },
            {
                5, new ExistReason(ExitCodeTypes.UserCanceled)
            },
            {
                7, new ExistReason(ExitCodeTypes.RequirementUnmet, "The Preparing to Install stage determined that Setup cannot proceed with installation.")
            },
            {
                8,
                new ExistReason(ExitCodeTypes.RestartRequired,
                    "The Preparing to Install stage determined that Setup cannot proceed with installation, and that the system needs to be restarted in order to correct the problem.")
            },
            {
                3010, new ExistReason(ExitCodeTypes.RestartRequired, null, true)
            },
        };

        public InnoWhisperer(IProcessController processController, IPathResolver pathResolver, Logger logger)
            : base(processController, pathResolver, logger)
        {
        }

        protected override InstallMethodTypes InstallMethod => InstallMethodTypes.Inno;

        //Command line args: http://www.jrsoftware.org/ishelp/index.php?topic=setupcmdline
        protected override string InteractiveArgs => "";
        protected override string PassiveArgs => "/SILENT /SUPPRESSMSGBOXES /NORESTART /CLOSEAPPLICATIONS /FORCECLOSEAPPLICATIONS /RESTARTEXITCODE=3010";
        protected override string SilentArgs => "/VERYSILENT /SUPPRESSMSGBOXES /NORESTART /CLOSEAPPLICATIONS /FORCECLOSEAPPLICATIONS /RESTARTEXITCODE=3010";
        protected override string LogArgs => "/LOG={path}";
    }
}