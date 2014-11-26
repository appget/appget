using AppGet.HostSystem;
using AppGet.Manifests;

namespace AppGet.Requirements.Specifications
{
    public class MaxOsVersionSpecification : IEnforceRequirements
    {
        private readonly IEnvironmentProxy _environmentProxy;

        public MaxOsVersionSpecification(IEnvironmentProxy environmentProxy)
        {
            _environmentProxy = environmentProxy;
        }

        public EnforcementResult IsRequirementSatisfied(Installer installer)
        {
            if (installer.MaxWindowsVersion == null) return EnforcementResult.Pass();

            if (_environmentProxy.WindowsVersion <= installer.MaxWindowsVersion)
            {
                return EnforcementResult.Pass();
            }

            //TODO: make this the print the proper widnows version eg. Windows 7/Windows XP vs 6.2
            return EnforcementResult.Fail("Max supported OS version: {0}. Current version: {1}",
                                                        installer.MaxWindowsVersion,
                                                        _environmentProxy.WindowsVersion.Version);
        }
    }
}
