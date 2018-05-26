using AppGet.HostSystem;
using AppGet.Manifest;
using AppGet.Manifests;

namespace AppGet.Requirements.Specifications
{
    public class OsArchitectureSpecification : IEnforceRequirements
    {
        private readonly IEnvInfo _envInfo;

        public OsArchitectureSpecification(IEnvInfo envInfo)
        {
            _envInfo = envInfo;
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
                    if (_envInfo.Is64BitOperatingSystem)
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