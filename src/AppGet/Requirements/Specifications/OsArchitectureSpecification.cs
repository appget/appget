using AppGet.HostSystem;
using AppGet.Manifests;

namespace AppGet.Requirements.Specifications
{
    public class OsArchitectureSpecification : IEnforceRequirements
    {
        private readonly IOsInfo _osInfo;

        public OsArchitectureSpecification(IOsInfo osInfo)
        {
            _osInfo = osInfo;
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
                        if (_osInfo.Is64BitOperatingSystem)
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
