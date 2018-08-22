using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AppGet.Crypto.Hash;
using AppGet.Infrastructure.Events;
using AppGet.ProgressTracker;
using NLog;

namespace AppGet.FileTransfer
{
    public interface IFileTransferService
    {
        Task<string> TransferFile(string source, string destinationFolder, string sha256);
        Task<string> ReadContent(string source);
    }

    public class FileTransferStartedEvent : StatusUpdateEvent
    {
        public string Source { get; }
        public string Destination { get; }

        public FileTransferStartedEvent(object sender, string source, string destination)
            : base(sender)
        {
            Source = source;
            Destination = destination;
        }
    }

    public class FileTransferCompletedEvent : StatusUpdateEvent
    {
        public string Source { get; }
        public string Destination { get; }

        public FileTransferCompletedEvent(object sender, string source, string destination)
            : base(sender)
        {
            Source = source;
            Destination = destination;
        }
    }

    public class FileTransferService : IFileTransferService
    {
        private readonly IEnumerable<IFileTransferClient> _transferClients;
        private readonly ITransferCacheService _transferCacheService;
        private readonly IChecksumService _checksumService;
        private readonly ITinyMessengerHub _tinyMessengerHub;
        private readonly Logger _logger;

        public FileTransferService(IEnumerable<IFileTransferClient> transferClients, ITransferCacheService transferCacheService,
            IChecksumService checksumService, ITinyMessengerHub tinyMessengerHub, Logger logger)
        {
            _transferClients = transferClients;
            _transferCacheService = transferCacheService;
            _checksumService = checksumService;
            _tinyMessengerHub = tinyMessengerHub;
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

        public async Task<string> TransferFile(string source, string destinationFolder, string sha256)
        {
            _tinyMessengerHub.PublishAsync(new FileTransferStartedEvent(this, source, destinationFolder));
            _logger.Debug($"Transfering file from {source} to {destinationFolder}");
            var client = GetClient(source);
            var fileName = await client.GetFileName(source);
            var destinationPath = Path.Combine(destinationFolder, fileName);

            if (_transferCacheService.IsValid(destinationPath, sha256))
            {
                _logger.Info("Skipping download. Using already downloaded file.");
            }
            else
            {
                var progressCallback = new Action<ProgressState>(p => _tinyMessengerHub.Publish(new GenericTinyMessage<ProgressState>(this, p)));

                Console.WriteLine();
                _logger.Info($"Downloading installer from {source}");
                await client.TransferFile(source, destinationPath, progressCallback);
                _logger.Debug($"Installer downloaded to {destinationPath}");

                if (sha256 == null)
                {
                    _logger.Debug("No hash provided. skipping checksum validation");
                }
                else
                {
                    _checksumService.ValidateHash(destinationPath, sha256);
                }
            }

            _tinyMessengerHub.PublishAsync(new FileTransferCompletedEvent(this, source, destinationFolder));

            return destinationPath;
        }

        public async Task<string> ReadContent(string source)
        {
            var client = GetClient(source);
            return await client.ReadString(source);
        }
    }
}