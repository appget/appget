using AppGet.HostSystem;
using AppGet.Manifest;

namespace AppGet.Installers.Requirements.Specifications
{
    public class MinOsVersionSpecification : IEnforceRequirements
    {
        private readonly IEnvInfo _environmentProxy;

        public MinOsVersionSpecification(IEnvInfo environmentProxy)
        {
            _environmentProxy = environmentProxy;
        }

        public EnforcementResult IsRequirementSatisfied(Installer installer)
        {
            if (installer.MinWindowsVersion == null) return EnforcementResult.Pass();

            if (_environmentProxy.WindowsVersion >= installer.MinWindowsVersion)
            {
                return EnforcementResult.Pass();
            }

            return EnforcementResult.Fail($"Min supported OS version: {installer.MinWindowsVersion}. Current version: {_environmentProxy.WindowsVersion}");
        }
    }
}