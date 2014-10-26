using AppGet.Commands.Install;
using AppGet.Commands.Uninstall;
using AppGet.FlightPlans;

namespace AppGet.Installers
{
    public interface IInstallerWhisperer
    {
        void Install(string installerLocation, FlightPlan flightPlan, InstallOptions installOptions);
        void Uninstall(FlightPlan flightPlan, UninstallOptions installOptions);
        bool CanHandle(InstallMethodType installMethod);
    }
}