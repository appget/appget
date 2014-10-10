using AppGet.Environment;
using AppGet.FlightPlans;

namespace AppGet.Requirements.Specifications
{
    public class OsArchitectureSpecification : IEnforceRequirements
    {
        private readonly IEnvironmentProxy _environmentProxy;

        public OsArchitectureSpecification(IEnvironmentProxy environmentProxy)
        {
            _environmentProxy = environmentProxy;
        }

        public EnforcementResult IsRequirementSatisfied(PackageSource packageSource)
        {
            if (packageSource.Architecture == ArchitectureType.x86)
            {
                return new EnforcementResult(true);
            }

            if (packageSource.Architecture == ArchitectureType.x64)
            {
                if (_environmentProxy.Is64BitOperatingSystem)
                {
                    return new EnforcementResult(true);
                }

                return new EnforcementResult(false, "x64 OS required for installation");
            }

            if (packageSource.Architecture == ArchitectureType.ARM)
            {
                return new EnforcementResult(false, "ARM is not supported at this time");
            }

            if (packageSource.Architecture == ArchitectureType.Itanium)
            {
                return new EnforcementResult(false, "Itanium is not supported at this time");
            }

            return new EnforcementResult(false, "Unsupported architecture: " + packageSource.Architecture);
        }
    }
}
