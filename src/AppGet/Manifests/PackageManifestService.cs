using System;
using System.IO;
using AppGet.AppData;
using AppGet.CreatePackage;
using AppGet.FileSystem;
using AppGet.FileTransfer;
using AppGet.Manifest;
using AppGet.Manifest.Builder;
using AppGet.Manifest.Serialization;
using NLog;

namespace AppGet.Manifests
{
    public interface IPackageManifestService
    {
        PackageManifest LoadManifest(string source);
        string WriteManifest(PackageManifestBuilder manifestBuilder);
        void PrintManifest(PackageManifest manifest);
    }

    public class PackageManifestService : IPackageManifestService
    {
        private readonly IFileTransferService _fileTransferService;
        private readonly IFileSystem _fileSystem;
        private readonly IConfig _config;
        private readonly Logger _logger;

        public PackageManifestService(IFileTransferService fileTransferService, IFileSystem fileSystem, IConfig config, Logger logger)
        {
            _fileTransferService = fileTransferService;
            _fileSystem = fileSystem;
            _config = config;
            _logger = logger;
        }

        public PackageManifest LoadManifest(string source)
        {
            _logger.Info($"Loading package manifest from {source}");
            var text = _fileTransferService.ReadContentAsync(source);

            return Yaml.Deserialize<PackageManifest>(text);
        }

        public string WriteManifest(PackageManifestBuilder manifestBuilder)
        {
            var manifest = manifestBuilder.Build();
            var manifestName = $"{manifest.Id}.{manifestBuilder.Tag}".Trim('.');
            var fileName = $"{manifestName}.yaml";
            var applicationManifestDir = Path.Combine(_config.LocalRepository, manifest.Id);
            var manifestPath = Path.Combine(applicationManifestDir, fileName);
            _fileSystem.CreateDirectory(applicationManifestDir);

            _fileSystem.WriteAllText(manifestPath, Yaml.Serialize(manifest));

            _logger.Info($"Package manifest was saved to {manifestPath}");

            return manifestPath;
        }

        public void Submit(PackageManifestBuilder builder)
        {
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
    }
}