using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AppGet.CommandLine.Prompts;
using AppGet.Compression;
using AppGet.Installers;
using AppGet.Manifests;
using AppGet.Tools;
using NLog;

namespace AppGet.CreatePackage.ManifestPopulators
{
    public class PopulateInstallMethod : IPopulateManifest
    {
        private readonly IEnumerable<IDetectInstallMethod> _installMethodDetectors;
        private readonly ICompressionService _compressionService;
        private readonly ISigCheck _sigCheck;
        private readonly Logger _logger;

        public PopulateInstallMethod(IEnumerable<IDetectInstallMethod> installMethodDetectors, ICompressionService compressionService, ISigCheck sigCheck, Logger logger)
        {
            _installMethodDetectors = installMethodDetectors;
            _compressionService = compressionService;
            _sigCheck = sigCheck;
            _logger = logger;
        }

        public void Populate(PackageManifestBuilder manifest, FileVersionInfo fileVersionInfo, bool interactive)
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

                    _logger.Info("Installer was detected as " + manifest.InstallMethod.Top);
                }
            }

            if (interactive && manifest.InstallMethod.HasConfidence(Confidence.Reasonable))
            {
                var methodPrompt = new EnumPrompt<InstallMethodTypes>();
                manifest.InstallMethod.Add(methodPrompt.Request("Installer", InstallMethodTypes.Custom), Confidence.Reasonable, this);
            }
        }
    }
}

