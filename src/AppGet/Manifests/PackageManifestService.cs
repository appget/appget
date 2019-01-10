using System;
using System.IO;
using System.Threading.Tasks;
using AppGet.AppData;
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
        Task<PackageManifest> LoadManifest(string source);
        string WriteManifest(PackageManifestBuilder manifestBuilder);
        void PrintManifest(PackageManifest manifest);
    }

    public class PackageManifestService : IPackageManifestService
    {
        private readonly IFileTransferService _fileTransferService;
        private readonly IFileSystem _fileSystem;
        private readonly IStore<Config> _configStore;
        private readonly Logger _logger;

        public PackageManifestService(IFileTransferService fileTransferService, IFileSystem fileSystem, IStore<Config> configStore, Logger logger)
        {
            _fileTransferService = fileTransferService;
            _fileSystem = fileSystem;
            _configStore = configStore;
            _logger = logger;
        }

        public async Task<PackageManifest> LoadManifest(string source)
        {
            _logger.Info($"Loading package manifest from {source}");
            var text = await _fileTransferService.ReadContent(source);

            var manifest = Yaml.Deserialize<PackageManifest>(text);
            return manifest;
        }

        public string WriteManifest(PackageManifestBuilder manifestBuilder)
        {
            var manifest = manifestBuilder.Build();
            var fileName = $"{manifest.GetFileName()}.yaml";
            var config = _configStore.Load();
            var applicationManifestDir = Path.Combine(config.LocalRepository, manifest.Id);
            var manifestPath = Path.Combine(applicationManifestDir, fileName);
            _fileSystem.CreateDirectory(applicationManifestDir);

            _fileSystem.WriteAllText(manifestPath, manifest.ToYaml());

            _logger.Info($"Package manifest was saved to {manifestPath}");

            return manifestPath;
        }

        public void PrintManifest(PackageManifest manifest)
        {
            Console.WriteLine("===============================================");
            Console.WriteLine();
            Console.WriteLine(manifest.ToYaml());
            Console.WriteLine();
            Console.WriteLine("===============================================");
        }
    }
}