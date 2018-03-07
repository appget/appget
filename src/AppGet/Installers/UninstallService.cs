using System.Collections.Generic;
using System.Linq;
using AppGet.Commands.Uninstall;
using AppGet.Manifests;
using NLog;

namespace AppGet.Installers
{
    public interface IUninstallService
    {
        void Uninstall(PackageManifest packageManifest, UninstallOptions installOptions);
    }

    public class UninstallService : IUninstallService
    {
        private readonly Logger _logger;
        private readonly List<IInstallerWhisperer> _installWhisperers;

        public UninstallService(Logger logger, List<IInstallerWhisperer> installWhisperers)
        {
            _logger = logger;
            _installWhisperers = installWhisperers;
        }

        public void Uninstall(PackageManifest packageManifest, UninstallOptions installOptions)
        {
            _logger.Info("Beginning uninstallation of " + packageManifest.Id);

            var whisperer = _installWhisperers.Single(c => c.CanHandle(packageManifest.InstallMethod));
        }
    }
}