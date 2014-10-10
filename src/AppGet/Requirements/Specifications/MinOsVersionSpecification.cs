using System;
using AppGet.Environment;
using AppGet.FlightPlans;

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

            if (_environmentProxy.OSVersion.Version >= packageSource.MinWindowsVersion)
            {
                return EnforcementResult.Pass();
            }

            return EnforcementResult.Fail("Min supported OS version: {0}. Current version: {1}",
                                                        packageSource.MaxWindowsVersion,
                                                        _environmentProxy.OSVersion.Version);
        }
    }
}
