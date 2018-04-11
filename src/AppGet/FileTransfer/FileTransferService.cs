using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AppGet.Crypto.Hash;
using AppGet.Manifests;
using AppGet.ProgressTracker;
using NLog;

namespace AppGet.FileTransfer
{
    public interface IFileTransferService
    {
        string TransferFile(string source, string destinationFolder, FileVerificationInfo fileVerificationInfo);
        string ReadContentAsync(string source);
    }

    public class FileTransferService : IFileTransferService
    {
        private readonly IEnumerable<IFileTransferClient> _transferClients;
        private readonly ITransferCacheService _transferCacheService;
        private readonly IChecksumService _checksumService;
        private readonly Logger _logger;

        public FileTransferService(IEnumerable<IFileTransferClient> transferClients, ITransferCacheService transferCacheService, IChecksumService checksumService, Logger logger)
        {
            _transferClients = transferClients;
            _transferCacheService = transferCacheService;
            _checksumService = checksumService;
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

        public  string TransferFile(string source, string destinationFolder, FileVerificationInfo fileVerificationInfo)
        {
            _logger.Debug($"Transfering file from {source} to {destinationFolder}");
            var client = GetClient(source);
            var fileName =  client.GetFileName(source);
            var destinationPath = Path.Combine(destinationFolder, fileName);

            if (_transferCacheService.IsValid(destinationPath, fileVerificationInfo))
            {
                _logger.Info("Skipping download. Using already downloaded file.");
            }
            else
            {
                client.OnStatusUpdated = ConsoleProgressReporter.HandleProgress;
                client.OnCompleted = ConsoleProgressReporter.HandleCompleted;

                Console.WriteLine();
                _logger.Info($"Downloading installer from {source}");
                client.TransferFile(source, destinationPath);
                _logger.Debug($"Installer downloaded to {destinationPath}");

                if (fileVerificationInfo.HashValue == null)
                {
                    _logger.Debug("No hash provided. skipping checksum validation");
                }
                else
                {
                    _checksumService.ValidateHash(destinationPath, fileVerificationInfo);
                }
            }

            return destinationPath;
        }

        public  string ReadContentAsync(string source)
        {
            var client = GetClient(source);
            return  client.ReadString(source);
        }
    }
}
