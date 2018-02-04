using System.Collections.Generic;
using System.IO;
using System.Linq;
using AppGet.Manifests;
using AppGet.ProgressTracker;
using NLog;

namespace AppGet.FileTransfer
{
    public interface IFileTransferService
    {
        string TransferFile(string source, string destinationFolder, FileHash fileHash);
        string ReadContent(string source);
    }

    public class FileTransferService : IFileTransferService
    {
        private readonly IEnumerable<IFileTransferClient> _transferClients;
        private readonly ITransferCacheService _transferCacheService;
        private readonly Logger _logger;

        public FileTransferService(IEnumerable<IFileTransferClient> transferClients, ITransferCacheService transferCacheService, Logger logger)
        {
            _transferClients = transferClients;
            _transferCacheService = transferCacheService;
            _transferCacheService = transferCacheService;
            _logger = logger;
        }


        private IFileTransferClient GetClient(string source)
        {
            var client = _transferClients.SingleOrDefault(c => c.CanHandleProtocol(source));

            if (client == null)
            {
                _logger.Debug($"Unable to handle protocol for: {source} - Unknown Protocol");

                throw new ProtocolNotSupportedException($"Unable to handle download for: {source} - Unknown Protocol");
            }

            return client;
        }

        public string TransferFile(string source, string destinationFolder, FileHash fileHash)
        {
            var client = GetClient(source);
            var destinationPath = Path.Combine(destinationFolder, client.GetFileName(source));

            if (_transferCacheService.IsValid(destinationPath, fileHash))
            {
                _logger.Info("Skipping download");
            }
            else
            {
                client.OnStatusUpdated = ConsoleProgressReporter.HandleProgress;
                client.OnCompleted = ConsoleProgressReporter.HandleCompleted;

                _logger.Info($"Downloading installer from {source}");
                client.TransferFile(source, destinationPath);
                _logger.Info($"Installer downloaded to {destinationPath}");
            }

            return destinationPath;
        }

        public string ReadContent(string source)
        {
            var client = GetClient(source);
            return client.ReadString(source);
        }
    }
}
