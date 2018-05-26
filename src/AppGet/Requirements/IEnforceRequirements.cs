using AppGet.Manifest;

namespace AppGet.Requirements
{
    public interface IEnforceRequirements
    {
        EnforcementResult IsRequirementSatisfied(Installer installer);
    }
}