using System;
using AppGet.FileTransfer;
using AppGet.PackageRepository;
using AppGet.Serialization;
using NLog;

namespace AppGet.Manifests
{
    public interface IPackageManifestService
    {
        PackageManifest LoadManifest(PackageInfo packageInfo);
        void PrintManifest(PackageManifest manifest);
    }

    public class PackageManifestService : IPackageManifestService
    {
        private readonly IFileTransferService _fileTransferService;
        private readonly Logger _logger;

        public PackageManifestService(IFileTransferService fileTransferService, Logger logger)
        {
            _fileTransferService = fileTransferService;
            _logger = logger;
        }


        public PackageManifest LoadManifest(PackageInfo packageInfo)
        {
            var text = ReadManifest(packageInfo);
            return Yaml.Deserialize<PackageManifest>(text);
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