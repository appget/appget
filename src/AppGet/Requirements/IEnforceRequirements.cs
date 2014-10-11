using AppGet.FlightPlans;

namespace AppGet.Requirements
{
    public interface IEnforceRequirements
    {
        EnforcementResult IsRequirementSatisfied(Installer installer);
    }
}
