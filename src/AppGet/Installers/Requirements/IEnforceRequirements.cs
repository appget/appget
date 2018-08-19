using AppGet.Manifest;

namespace AppGet.Installers.Requirements
{
    public interface IEnforceRequirements
    {
        EnforcementResult IsRequirementSatisfied(Installer installer);
    }
}