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
                default:
                    {
                        return EnforcementResult.Fail("Unsupported architecture: " + installer.Architecture);
                    }
            }
        }
    }
}
