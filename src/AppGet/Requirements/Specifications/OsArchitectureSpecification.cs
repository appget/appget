using AppGet.Environment;
using AppGet.FlightPlans;

namespace AppGet.Requirements.Specifications
{
    public class OsArchitectureSpecification : IEnforceRequirements
    {
        private readonly IEnvironmentProxy _environmentProxy;

        public OsArchitectureSpecification(IEnvironmentProxy environmentProxy)
        {
            _environmentProxy = environmentProxy;
        }

        public bool IsRequirementSatisfied(PackageSource packageSource)
        {
            if (packageSource.Architecture == ArchitectureType.x86) return true;

            if (packageSource.Architecture == ArchitectureType.x64)
            {
                return _environmentProxy.Is64BitOperatingSystem;
            }

            //TODO: Handle ARM and Itanium
            return false;
        }
    }
}
