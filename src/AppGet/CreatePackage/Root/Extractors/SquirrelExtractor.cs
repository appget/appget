using System;
using System.Linq;
using AppGet.Compression;
using AppGet.Installers.Squirrel;
using AppGet.Manifests;
using NLog;

namespace AppGet.CreatePackage.Root.Extractors
{
    public class SquirrelExtractor : IExtractToManifestRoot
    {
        private readonly ISquirrelReader _squirrelReader;
        private readonly ICompressionService _compressionService;
        private readonly Logger _logger;


        public SquirrelExtractor(ISquirrelReader squirrelReader, ICompressionService compressionService, Logger logger)
        {
            _squirrelReader = squirrelReader;
            _compressionService = compressionService;
            _logger = logger;
        }

        public void Invoke(PackageManifestBuilder manifest)
        {
            if (manifest.InstallMethod.Top != InstallMethodTypes.Squirrel) return;

            var arch = _compressionService.TryOpen(manifest.Installers.First().FilePath);

            if (arch == null) return;

            try
            {
                var nugetSpec = _squirrelReader.GetNugetSpec(arch);
                manifest.Name.Add(nugetSpec.Metadata.Title, Confidence.Authoritive, this);
                manifest.Version.Add(nugetSpec.Metadata.Version, Confidence.Authoritive, this);
                manifest.Id.Add(nugetSpec.Metadata.Id.ToLowerInvariant(), Confidence.Reasonable, this);
            }
            catch (Exception e)
            {
                _logger.Debug(e);
            }
        }
    }
}

