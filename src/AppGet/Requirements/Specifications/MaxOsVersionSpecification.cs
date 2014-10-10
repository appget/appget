using AppGet.FlightPlans;
using AppGet.HostSystem;

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

            if (_environmentProxy.WindowsVersion.Version <= packageSource.MaxWindowsVersion)
            {
                return EnforcementResult.Pass();
            }

            //TODO: make this the print the proper widnows version eg. Windows 7/Windows XP vs 6.2
            return EnforcementResult.Fail("Max supported OS version: {0}. Current version: {1}",
                                                        packageSource.MaxWindowsVersion,
                                                        _environmentProxy.WindowsVersion.Version);
        }
    }
}
