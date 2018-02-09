using System;
using System.IO;
using AppGet.FileSystem;
using AppGet.FileTransfer;
using AppGet.PackageRepository;
using AppGet.Serialization;
using NLog;

namespace AppGet.Manifests
{
    public interface IPackageManifestService
    {
        PackageManifest LoadManifest(PackageInfo packageInfo);
        string WriteManifest(PackageManifest manifest, string manifestRoot);
        void PrintManifest(PackageManifest manifest);
    }

    public class PackageManifestService : IPackageManifestService
    {
        private readonly IFileTransferService _fileTransferService;
        private readonly IFileSystem _fileSystem;
        private readonly Logger _logger;

        public PackageManifestService(IFileTransferService fileTransferService, IFileSystem fileSystem, Logger logger)
        {
            _fileTransferService = fileTransferService;
            _fileSystem = fileSystem;
            _logger = logger;
        }


        public PackageManifest LoadManifest(PackageInfo packageInfo)
        {
            var text = ReadManifest(packageInfo);
            return Yaml.Deserialize<PackageManifest>(text);
        }

        public string WriteManifest(PackageManifest manifest, string manifestRoot)
        {
            var fileName = $"{manifest.Id}-{manifest.MajorVersion}.yaml";
            var applicationManifestDir = Path.Combine(manifestRoot, manifest.Id);
            var manifestPath = Path.Combine(applicationManifestDir, fileName);
            _fileSystem.CreateDirectory(applicationManifestDir);

            _fileSystem.WriteAllText(manifestPath, Yaml.Serialize(manifest));

            _logger.Info($"Package manifest was saved to {manifestPath}");
            return manifestPath;

        }

        public void PrintManifest(PackageManifest manifest)
        {
            var text = Yaml.Serialize(manifest);

            Console.WriteLine("===============================================");
            Console.WriteLine();
            Console.WriteLine(text);
            Console.WriteLine();
            Console.WriteLine("===============================================");
        }

        private string ReadManifest(PackageInfo packageInfo)
        {
            _logger.Info("Loading manifest for " + packageInfo);
            var text = _fileTransferService.ReadContent(packageInfo.ManifestUrl);
            return text;
        }
    }
}