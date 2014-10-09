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

        public bool IsRequirementSatisfied(PackageSource packageSource)
        {
            if (packageSource.MinWindowsVersion == null) return true;

            return _environmentProxy.OSVersion.Version >= packageSource.MinWindowsVersion;
        }
    }
}
