using System.Collections.Generic;
using System.Linq;
using AppGet.Compression;
using AppGet.Installers;
using AppGet.Tools;
using NLog;

namespace AppGet.CreatePackage.Root.Extractors
{
    public class InstallMethodExtractor : IExtractToManifestRoot
    {
        private readonly IEnumerable<IDetectInstallMethod> _installMethodDetectors;
        private readonly ICompressionService _compressionService;
        private readonly ISigCheck _sigCheck;
        private readonly Logger _logger;

        public InstallMethodExtractor(IEnumerable<IDetectInstallMethod> installMethodDetectors, ICompressionService compressionService, ISigCheck sigCheck, Logger logger)
        {
            _installMethodDetectors = installMethodDetectors;
            _compressionService = compressionService;
            _sigCheck = sigCheck;
            _logger = logger;
        }

        public void Invoke(PackageManifestBuilder manifest)
        {
            _logger.Info("Detecting application installer");

            using (var archive = _compressionService.TryOpen(manifest.FilePath))
            {
                var exeManifest = _sigCheck.GetManifest(manifest.FilePath);

                if (archive != null || !string.IsNullOrWhiteSpace(exeManifest))
                {
                    var scores = _installMethodDetectors.ToDictionary(c => c,
                        c => c.GetConfidence(manifest.FilePath, archive, exeManifest));

                    foreach (var results in scores)
                    {
                        manifest.InstallMethod.Add(results.Key.InstallMethod, results.Value, results.Key);
                    }

                    _logger.Info("Installer was detected as " + manifest.InstallMethod.Value);
                }
            }
        }
    }
}

