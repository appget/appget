using AppGet.FlightPlans;

namespace AppGet.Requirements
{
    public interface IEnforceRequirements
    {
        EnforcementResult IsRequirementSatisfied(PackageSource packageSource);
    }
}
