using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AppGet.CommandLine.Prompts;
using AppGet.Compression;
using AppGet.Installers;
using AppGet.Manifests;
using NLog;

namespace AppGet.CreatePackage.ManifestPopulators
{
    public class PopulateInstallMethod : IPopulateManifest
    {
        private readonly IEnumerable<IDetectInstallMethod> _installMethodDetectors;
        private readonly ICompressionService _compressionService;
        private readonly Logger _logger;

        public PopulateInstallMethod(IEnumerable<IDetectInstallMethod> installMethodDetectors, ICompressionService compressionService, Logger logger)
        {
            _installMethodDetectors = installMethodDetectors;
            _compressionService = compressionService;
            _logger = logger;
        }

        public void Populate(PackageManifest manifest, FileVersionInfo fileVersionInfo)
        {
            _logger.Info("Detecting application installer");
            var installer = manifest.Installers.First();

            var archive = _compressionService.TryOpen(installer.FilePath);
            if (archive != null)
            {
                var scores = _installMethodDetectors.ToDictionary(c => c.InstallMethod,
                    c => c.GetConfidence(installer.FilePath, archive));

                var positives = scores.Values.Count(c => c != 0);

                if (positives == 1)
                {
                    manifest.InstallMethod = scores.Single(c => c.Value == 1).Key;
                    _logger.Info("Installer was detected as " + manifest.InstallMethod);
                    return;
                }
            }

            var methodPrompt = new EnumPrompt<InstallMethodTypes>();
            manifest.InstallMethod = methodPrompt.Request("Installer", InstallMethodTypes.Unknown);
        }
    }
}

