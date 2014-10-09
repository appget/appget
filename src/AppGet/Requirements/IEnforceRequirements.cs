using AppGet.FlightPlans;

namespace AppGet.Requirements
{
    public interface IEnforceRequirements
    {
        bool IsRequirementSatisfied(PackageSource packageSource);
    }
}
