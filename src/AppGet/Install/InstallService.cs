using AppGet.Commands.Install;
using AppGet.FlightPlans;
using NLog;

namespace AppGet.Install
{
    public interface IInstallService
    {
        void Install(string installerLocation, FlightPlan flightPlan, InstallOptions installOptions);
    }

    public class InstallService : IInstallService
    {
        private readonly Logger _logger;
        private readonly ZipInstaller _zipInstaller;

        public InstallService(Logger logger, ZipInstaller zipInstaller)
        {
            _logger = logger;
            _zipInstaller = zipInstaller;
        }

        public void Install(string installerLocation, FlightPlan flightPlan, InstallOptions installOptions)
        {
            _logger.Info("Begining installation of " + flightPlan.Id);
            _zipInstaller.Install(installerLocation, flightPlan, installOptions);
        }
    }
}
