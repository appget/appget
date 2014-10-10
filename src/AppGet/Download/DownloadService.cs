using System.Collections.Generic;
using System.Linq;
using AppGet.ProgressTracker;
using NLog;

namespace AppGet.Download
{
    public interface IDownloadService
    {
        void DownloadFile(string source, string destination);
    }

    public class DownloadService : IDownloadService
    {
        private readonly IEnumerable<IDownloadClient> _downloadClients;
        private readonly Logger _logger;

        public DownloadService(IEnumerable<IDownloadClient> downloadClients, Logger logger)
        {
            _downloadClients = downloadClients;
            _logger = logger;
        }

        public void DownloadFile(string source, string destination)
        {
            var client = _downloadClients.SingleOrDefault(c => c.CanHandleProtocol(source));

            if (client == null)
            {
                _logger.Debug("Unable to handle protocol for: {0} - Unknown Protocol", source);

                throw new ProtocolNotSupportedException("Unable to handle download for: {0} - Unknown Protocol", source);
            }

            client.OnStatusUpdates = ConsoleProgressReporter.HandleProgress;
            client.OnCompleted = ConsoleProgressReporter.HandleCompleted;

            _logger.Info("Downloading installer from " + source);
            client.DownloadFile(source, destination);
            _logger.Info("Installer downloaded to " + destination);
        }
    }
}
