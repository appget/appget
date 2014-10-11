using AppGet.FlightPlans;
using AppGet.HostSystem;

namespace AppGet.Requirements.Specifications
{
    public class OsArchitectureSpecification : IEnforceRequirements
    {
        private readonly IEnvironmentProxy _environmentProxy;

        public OsArchitectureSpecification(IEnvironmentProxy environmentProxy)
        {
            _environmentProxy = environmentProxy;
        }

        public EnforcementResult IsRequirementSatisfied(Installer installer)
        {
            switch (installer.Architecture)
            {
                case ArchitectureType.Any:
                    {
                        return EnforcementResult.Pass();
                    }
                case ArchitectureType.x86:
                    {
                        return EnforcementResult.Pass();
                    }
                case ArchitectureType.x64:
                    {
                        if (_environmentProxy.Is64BitOperatingSystem)
                        {
                            return EnforcementResult.Pass();
                        }
                        return EnforcementResult.Fail("x64 OS required for installation");
                    }
                case ArchitectureType.ARM:
                    {
                        return EnforcementResult.Fail("ARM is not supported at this time");
                    }
                case ArchitectureType.Itanium:
                    {
                        return EnforcementResult.Fail("Itanium is not supported at this time");
                    }
                default:
                    {
                        return EnforcementResult.Fail("Unsupported architecture: " + installer.Architecture);
                    }
            }
        }
    }
}
