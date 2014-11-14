using System.Collections.Generic;
using System.Linq;
using AppGet.Commands.Install;
using AppGet.Manifests;
using NLog;

namespace AppGet.Installers
{
    public interface IInstallService
    {
        void Install(string installerLocation, PackageManifest packageManifest, InstallOptions installOptions);
    }

    public class InstallService : IInstallService
    {
        private readonly Logger _logger;
        private readonly List<IInstallerWhisperer> _installWhisperers;

        public InstallService(Logger logger, List<IInstallerWhisperer> installWhisperers)
        {
            _logger = logger;
            _installWhisperers = installWhisperers;
        }

        public void Install(string installerLocation, PackageManifest packageManifest, InstallOptions installOptions)
        {
            _logger.Info("Beginning installation of [{0}] {1}", packageManifest.Id, packageManifest.Name);

            var whisperer = _installWhisperers.Single(c => c.CanHandle(packageManifest.InstallMethod));

            whisperer.Install(installerLocation, packageManifest, installOptions);

            _logger.Info("Installation completed for [{0}] {1}", packageManifest.Id, packageManifest.Name);
        }
    }
}
