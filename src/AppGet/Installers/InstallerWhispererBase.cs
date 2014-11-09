using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppGet.Commands.Install;
using AppGet.Commands.Uninstall;
using AppGet.FlightPlans;
using AppGet.Processes;
using NLog;

namespace AppGet.Installers
{
    public abstract class InstallerWhispererBase : IInstallerWhisperer
    {
        private readonly IProcessController _processController;
        private readonly Logger _logger;

        protected InstallerWhispererBase(IProcessController processController, Logger logger)
        {
            _processController = processController;
            _logger = logger;
        }

        public abstract void Install(string installerLocation, FlightPlan flightPlan, InstallOptions installOptions);
        public abstract void Uninstall(FlightPlan flightPlan, UninstallOptions installOptions);
        public abstract bool CanHandle(InstallMethodType installMethod);

        protected virtual int Execute(string exectuable, string args)
        {
            var process = _processController.Start(exectuable, args, OnOutputDataReceived, OnErrorDataReceived);

            _processController.WaitForExit(process);

            return process.ExitCode;
        }

        protected virtual void OnOutputDataReceived(string message)
        {
            _logger.Info(message);
        }

        protected virtual void OnErrorDataReceived(string message)
        {
            _logger.Error(message);
        }
    }
}
