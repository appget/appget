using System;
using AppGet.Commands.Install;
using AppGet.Commands.Uninstall;
using AppGet.FlightPlans;
using AppGet.HostSystem;
using AppGet.Processes;
using NLog;

namespace AppGet.Installers.Inno
{
    class InnoWhisperer : InstallerWhispererBase
    {
        private readonly IPathResolver _pathResolver;
        private readonly Logger _logger;

        public InnoWhisperer(IProcessController processController, IPathResolver pathResolver, Logger logger)
            : base(processController, logger)
        {
            _pathResolver = pathResolver;
            _logger = logger;
        }

        public override void Install(string installerLocation, FlightPlan flightPlan, InstallOptions installOptions)
        {
            //Command line args: http://www.jrsoftware.org/ishelp/index.php?topic=setupcmdline

            var logFile = _pathResolver.GetInstallerLogFile(flightPlan);
            var args = GetArgs(logFile);

            _logger.Debug("Writing Inno log files to {0}", logFile);

            Execute(installerLocation, args);
        }

        public override void Uninstall(FlightPlan flightPlan, UninstallOptions installOptions)
        {
            //Command line args: http://www.jrsoftware.org/ishelp/index.php?topic=uninstcmdline

            throw new NotImplementedException();
        }

        public override bool CanHandle(InstallMethodType installMethod)
        {
            return installMethod == InstallMethodType.Inno;
        }

        private static string GetArgs(string logFile)
        {
            //Sets the exit code to 3010 when a restart is required (same as Windows Installer)
            //Can also use: /CLOSEAPPLICATIONS /RESTARTAPPLICATIONS to stop then restart applications
            return String.Format("/VERYSILENT /SUPPRESSMSGBOXES /NORESTART /RESTARTEXITCODE=3010 /LOG=\"{0}\"", logFile);
        }
    }
}
