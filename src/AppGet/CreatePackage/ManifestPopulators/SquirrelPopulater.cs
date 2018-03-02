using AppGet.Manifests;
using NLog;
using System;
using System.Diagnostics;
using System.Linq;
using AppGet.Compression;
using AppGet.Installers.Squirrel;

namespace AppGet.CreatePackage.ManifestPopulators
{
    public class SquirrelPopulater : IPopulateManifest
    {
        private readonly ISquirrelReader _squirrelReader;
        private readonly ICompressionService _compressionService;
        private readonly Logger _logger;


        public SquirrelPopulater(ISquirrelReader squirrelReader, ICompressionService compressionService, Logger logger)
        {
            _squirrelReader = squirrelReader;
            _compressionService = compressionService;
            _logger = logger;
        }

        public void Populate(PackageManifestBuilder manifest, FileVersionInfo fileVersionInfo, bool interactive)
        {
            if (manifest.InstallMethod.Top != InstallMethodTypes.Squirrel) return;

            var arch = _compressionService.TryOpen(manifest.Installers.First().FilePath);

            if (arch == null) return;

            try
            {
                var nugetSpec = _squirrelReader.GetNugetSpec(arch);
                manifest.Name.Add(nugetSpec.Metadata.Title, Confidence.VeryHigh, this);
                manifest.Version.Add(nugetSpec.Metadata.Version, Confidence.VeryHigh, this);
                manifest.Id.Add(nugetSpec.Metadata.Id.ToLowerInvariant(), Confidence.Reasonable, this);
            }
            catch (Exception e)
            {
                _logger.Debug(e);
            }
        }
    }
}

