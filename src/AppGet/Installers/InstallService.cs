using System.Collections.Generic;
using System.Linq;
using AppGet.Commands.Install;
using AppGet.FlightPlans;
using AppGet.Installers.Msi;
using NLog;

namespace AppGet.Installers
{
    public interface IInstallService
    {
        void Install(string installerLocation, FlightPlan flightPlan, InstallOptions installOptions);
    }

    public class InstallService : IInstallService
    {
        private readonly Logger _logger;
        private readonly List<IInstallerWhisperer> _installWhisperers;

        public InstallService(Logger logger, List<IInstallerWhisperer> installWhisperers)
        {
            _logger = logger;
            _installWhisperers = installWhisperers;
        }

        public void Install(string installerLocation, FlightPlan flightPlan, InstallOptions installOptions)
        {
            _logger.Info("Beginning installation of " + flightPlan.Id);

            var whisperer = _installWhisperers.Single(c => c.CanHandle(flightPlan.InstallMethod));

            whisperer.Install(installerLocation, flightPlan, installOptions);
        }
    }
}
