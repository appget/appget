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
            if (packageSource.MaxWindowsVersion == null) return new EnforcementResult(true);

            if (_environmentProxy.OSVersion.Version <= packageSource.MaxWindowsVersion)
            {
                return new EnforcementResult(true);
            }

            return new EnforcementResult(false,
                                         String.Format("Max supported OS version: {0}. Current version: {1}",
                                                        packageSource.MaxWindowsVersion,
                                                        _environmentProxy.OSVersion.Version));
        }
    }
}
