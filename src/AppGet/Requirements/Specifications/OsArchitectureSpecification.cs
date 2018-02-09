using AppGet.HostSystem;
using AppGet.Manifests;

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
                case ArchitectureTypes.Any:
                    {
                        return EnforcementResult.Pass();
                    }
                case ArchitectureTypes.x86:
                    {
                        return EnforcementResult.Pass();
                    }
                case ArchitectureTypes.x64:
                    {
                        if (_environmentProxy.Is64BitOperatingSystem)
                        {
                            return EnforcementResult.Pass();
                        }
                        return EnforcementResult.Fail("x64 OS required for installation");
                    }
                case ArchitectureTypes.ARM:
                    {
                        return EnforcementResult.Fail("ARM is not supported at this time");
                    }
                case ArchitectureTypes.Itanium:
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
