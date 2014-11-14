using AppGet.Manifests;

namespace AppGet.Requirements
{
    public interface IEnforceRequirements
    {
        EnforcementResult IsRequirementSatisfied(Installer installer);
    }
}
