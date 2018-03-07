using AppGet.Commands.Install;
using AppGet.Compression;
using AppGet.FileSystem;
using AppGet.HostSystem;
using AppGet.Manifests;
using NLog;

namespace AppGet.Installers.Zip
{
    public class ZipWhisperer : IInstallerWhisperer
    {
        private readonly Logger _logger;
        private readonly ICompressionService _compressionService;
        private readonly IPathResolver _pathResolver;
        private readonly IFileSystem _fileSystem;

        public ZipWhisperer(Logger logger, ICompressionService compressionService, IPathResolver pathResolver, IFileSystem fileSystem)
        {
            _logger = logger;
            _compressionService = compressionService;
            _pathResolver = pathResolver;
            _fileSystem = fileSystem;
        }

        public void Install(string installerLocation, PackageManifest packageManifest, InstallOptions installOptions)
        {
            var target = _pathResolver.GetInstallationPath(packageManifest);
            _compressionService.Decompress(installerLocation, target);
        }

        public bool CanHandle(InstallMethodTypes installMethod)
        {
            return installMethod == InstallMethodTypes.Zip;
        }
    }
}