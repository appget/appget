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

        public bool IsRequirementSatisfied(PackageSource packageSource)
        {
            if (packageSource.MaxWindowsVersion == null) return true;

            return _environmentProxy.OSVersion.Version <= packageSource.MaxWindowsVersion;
        }
    }
}
