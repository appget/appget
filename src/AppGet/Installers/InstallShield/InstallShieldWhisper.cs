using System;
using AppGet.Commands.Install;
using AppGet.Commands.Uninstall;
using AppGet.FlightPlans;
using AppGet.HostSystem;
using AppGet.Processes;
using NLog;

namespace AppGet.Installers.InstallShield
{
    public class InstallShieldWhisperer : InstallerWhispererBase
    {
        private readonly IPathResolver _pathResolver;
        private readonly Logger _logger;

        public InstallShieldWhisperer(IProcessController processController, IPathResolver pathResolver, Logger logger)
            : base(processController, logger)
        {
            _pathResolver = pathResolver;
            _logger = logger;
        }

        public override void Install(string installerLocation, FlightPlan flightPlan, InstallOptions installOptions)
        {
            var logFile = _pathResolver.GetInstallerLogFile(flightPlan);
            var args = GetArgs(installerLocation, logFile);

            _logger.Debug("Writing log files to {0}", logFile);

            Execute(installerLocation, args);
        }

        public override void Uninstall(FlightPlan flightPlan, UninstallOptions installOptions)
        {
            throw new NotImplementedException();
        }

        public override bool CanHandle(InstallMethodType installMethod)
        {
            return installMethod == InstallMethodType.InstallShield;
        }

        private static string GetArgs(string msiPath, string logFile)
        {
            return String.Format("/s /v\"/quiet /norestart /Liwemoar! {1}\"", msiPath, logFile);
        }
    }
}
