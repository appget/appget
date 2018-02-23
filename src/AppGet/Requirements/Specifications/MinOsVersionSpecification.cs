using AppGet.HostSystem;
using AppGet.Manifests;

namespace AppGet.Requirements.Specifications
{
    public class MinOsVersionSpecification : IEnforceRequirements
    {
        private readonly IOsInfo _environmentProxy;

        public MinOsVersionSpecification(IOsInfo environmentProxy)
        {
            _environmentProxy = environmentProxy;
        }

        public EnforcementResult IsRequirementSatisfied(Installer installer)
        {
            if (installer.MinWindowsVersion == null) return EnforcementResult.Pass();

            if (_environmentProxy.Version >= installer.MinWindowsVersion)
            {
                return EnforcementResult.Pass();
            }

            return EnforcementResult.Fail("Min supported OS version: {0}. Current version: {1}",
                                                        installer.MinWindowsVersion,
                                                        _environmentProxy.Version);
        }
    }
}
