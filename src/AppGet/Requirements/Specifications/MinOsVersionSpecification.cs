using System;
using AppGet.HostSystem;
using AppGet.Manifests;

namespace AppGet.Requirements.Specifications
{
    public class MinOsVersionSpecification : IEnforceRequirements
    {
        private readonly IEnvironmentProxy _environmentProxy;

        public MinOsVersionSpecification(IEnvironmentProxy environmentProxy)
        {
            _environmentProxy = environmentProxy;
        }

        public EnforcementResult IsRequirementSatisfied(Installer installer)
        {
            if (installer.MinWindowsVersion == null) return EnforcementResult.Pass();

            if (_environmentProxy.WindowsVersion.Version >= installer.MinWindowsVersion)
            {
                return EnforcementResult.Pass();
            }

            return EnforcementResult.Fail("Min supported OS version: {0}. Current version: {1}",
                                                        installer.MaxWindowsVersion,
                                                        _environmentProxy.WindowsVersion.Version);
        }
    }
}
