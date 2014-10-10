using AppGet.FlightPlans;

namespace AppGet.PackageProvider
{
    public interface IPackageProvider
    {
        FlightPlan GetFlightPlan(string name);
    }
}