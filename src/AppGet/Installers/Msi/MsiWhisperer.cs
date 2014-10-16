using AppGet.Commands.Install;
using AppGet.Commands.Uninstall;
using AppGet.FlightPlans;
using AppGet.HostSystem;
using AppGet.Processes;
using NLog;

namespace AppGet.Installers.Msi
{
    public class MsiWhisperer : IInstallerWhisperer
    {
        private readonly IProcessController _processController;
        private readonly IPathResolver _pathResolver;
        private readonly Logger _logger;

        public MsiWhisperer(IProcessController processController, IPathResolver pathResolver, Logger logger)
        {
            _processController = processController;
            _pathResolver = pathResolver;
            _logger = logger;
        }

        public void Install(string installerLocation, FlightPlan flightPlan, InstallOptions installOptions)
        {
            var logFile = _pathResolver.GetInstallerLogFile(flightPlan);

            _logger.Debug("Writing MSI log files to {0}", logFile);

            var args = GetArgs(installerLocation, logFile);

            var process = _processController.Start("msiexec", args, OnOutputDataReceived, OnErrorDataReceived);



            _processController.WaitForExit(process);
        }

        public void Unnstall(FlightPlan flightPlan, UninstallOptions installOptions)
        {
            throw new System.NotImplementedException();
        }

        private static string GetArgs(string msiPath, string logFile)
        {
            return string.Format("/i {0} /quiet /norestart /Liwemoar! {1} ", msiPath, logFile);
        }

        public bool CanHandle(InstallMethodType installMethod)
        {
            return installMethod == InstallMethodType.MSI;
        }

        private void OnOutputDataReceived(string message)
        {
            _logger.Info(message);
        }

        private void OnErrorDataReceived(string message)
        {
            _logger.Error(message);
        }
    }
}
