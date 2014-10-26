using System.Collections.Generic;
using System.Linq;
using AppGet.Commands.Install;
using AppGet.Commands.Uninstall;
using AppGet.FlightPlans;
using NLog;

namespace AppGet.Installers
{
    public interface IUninstallService
    {
        void Uninstall(FlightPlan flightPlan, UninstallOptions installOptions);
    }

    public class UninstallService : IUninstallService
    {
        private readonly Logger _logger;
        private readonly List<IInstallerWhisperer> _installWhisperers;

        public UninstallService(Logger logger, List<IInstallerWhisperer> installWhisperers)
        {
            _logger = logger;
            _installWhisperers = installWhisperers;
        }

        public void Uninstall(FlightPlan flightPlan, UninstallOptions installOptions)
        {
            _logger.Info("Beginning uninstallation of " + flightPlan.Id);

            var whisperer = _installWhisperers.Single(c => c.CanHandle(flightPlan.InstallMethod));

            whisperer.Uninstall(flightPlan, installOptions);
        }
    }
}