using System.Collections.Generic;
using System.Linq;
using AppGet.ProgressTracker;
using NLog;

namespace AppGet.FileTransfer
{
    public interface IFileTransferService
    {
        void TransferFile(string source, string destination);
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

        public void TransferFile(string source, string destination)
        {
            var client = GetClient(source);

            client.OnStatusUpdates = ConsoleProgressReporter.HandleProgress;
            client.OnCompleted = ConsoleProgressReporter.HandleCompleted;

            _logger.Info("Downloading installer from " + source);
            client.TransferFile(source, destination);
            _logger.Info("InstallMethod downloaded to " + destination);
        }

        public string ReadContent(string source)
        {
            var client = GetClient(source);
            return client.ReadString(source);
        }
    }
}
