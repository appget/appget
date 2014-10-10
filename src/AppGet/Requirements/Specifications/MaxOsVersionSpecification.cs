using System;
using AppGet.Environment;
using AppGet.FlightPlans;

namespace AppGet.Requirements.Specifications
{
    public class MaxOsVersionSpecification : IEnforceRequirements
    {
        private readonly IEnvironmentProxy _environmentProxy;

        public MaxOsVersionSpecification(IEnvironmentProxy environmentProxy)
        {
            _environmentProxy = environmentProxy;
        }

        public EnforcementResult IsRequirementSatisfied(PackageSource packageSource)
        {
            if (packageSource.MaxWindowsVersion == null) return EnforcementResult.Pass();

            if (_environmentProxy.OSVersion.Version <= packageSource.MaxWindowsVersion)
            {
                return EnforcementResult.Pass();
            }

            return EnforcementResult.Fail("Max supported OS version: {0}. Current version: {1}",
                                                        packageSource.MaxWindowsVersion,
                                                        _environmentProxy.OSVersion.Version);
        }
    }
}
