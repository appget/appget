using System;
using AppGet.FlightPlans;
using AppGet.HostSystem;

namespace AppGet.Requirements.Specifications
{
    public class MinOsVersionSpecification : IEnforceRequirements
    {
        private readonly IEnvironmentProxy _environmentProxy;

        public MinOsVersionSpecification(IEnvironmentProxy environmentProxy)
        {
            _environmentProxy = environmentProxy;
        }

        public EnforcementResult IsRequirementSatisfied(PackageSource packageSource)
        {
            if (packageSource.MinWindowsVersion == null) return EnforcementResult.Pass();

            if (_environmentProxy.WindowsVersion.Version >= packageSource.MinWindowsVersion)
            {
                return EnforcementResult.Pass();
            }

            return EnforcementResult.Fail("Min supported OS version: {0}. Current version: {1}",
                                                        packageSource.MaxWindowsVersion,
                                                        _environmentProxy.WindowsVersion.Version);
        }
    }
}
