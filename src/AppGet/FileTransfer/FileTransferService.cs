using System.Collections.Generic;
using System.Linq;
using AppGet.ProgressTracker;
using NLog;

namespace AppGet.FileTransfer
{
    public interface IFileTransferService
    {
        string TransferFile(string source, string destinationFolder);
        string ReadContent(string source);
    }

    public class FileTransferService : IFileTransferService
    {
        private readonly IEnumerable<IFileTransferClient> _transferClients;
        private readonly Logger _logger;

        public FileTransferService(IEnumerable<IFileTransferClient> transferClients, Logger logger)
        {
            _transferClients = transferClients;
            _logger = logger;
        }


        private IFileTransferClient GetClient(string source)
        {
            var client = _transferClients.SingleOrDefault(c => c.CanHandleProtocol(source));

            if (client == null)
            {
                _logger.Debug("Unable to handle protocol for: {0} - Unknown Protocol", source);

                throw new ProtocolNotSupportedException("Unable to handle download for: {0} - Unknown Protocol", source);
            }

            return client;
        }

        public string TransferFile(string source, string destinationFolder)
        {
            var client = GetClient(source);

            client.OnStatusUpdated = ConsoleProgressReporter.HandleProgress;
            client.OnCompleted = ConsoleProgressReporter.HandleCompleted;

            _logger.Info("Downloading installer from " + source);
            var transferedFilePath = client.TransferFile(source, destinationFolder);
            _logger.Info("Installer downloaded to " + transferedFilePath);

            return transferedFilePath;
        }

        public string ReadContent(string source)
        {
            var client = GetClient(source);
            return client.ReadString(source);
        }
    }
}
