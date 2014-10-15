using AppGet.Commands.Install;
using AppGet.FlightPlans;

namespace AppGet.Installers
{
    public interface IInstallerWhisperer
    {
        void Install(string installerLocation, FlightPlan flightPlan, InstallOptions installOptions);
        bool CanHandle(InstallMethodType installMethod);
    }
}