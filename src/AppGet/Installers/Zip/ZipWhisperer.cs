using AppGet.Commands.Install;
using AppGet.Compression;
using AppGet.HostSystem;
using AppGet.Manifest;

namespace AppGet.Installers.Zip
{
    public class ZipWhisperer : IInstallerWhisperer
    {
        private readonly ICompressionService _compressionService;
        private readonly IPathResolver _pathResolver;

        public ZipWhisperer(ICompressionService compressionService, IPathResolver pathResolver)
        {
            _compressionService = compressionService;
            _pathResolver = pathResolver;
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