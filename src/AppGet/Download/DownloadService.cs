using System.Collections.Generic;
using System.Linq;
using NLog;

namespace AppGet.Download
{
    public interface IDownloadService
    {
        void DownloadFile(string url, string destination);
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

        public void DownloadFile(string url, string destination)
        {
            var client = _downloadClients.SingleOrDefault(c => c.CanHandleProtocol(url));

            if (client == null)
            {
                _logger.Debug("Unable to handle protocol for: {0} - Unknown Protocol", url);

                throw new ProtocolNotSupportedException("Unable to handle download for: {0} - Unknown Protocol", url);
            }

            client.DownloadFile(url, destination);
        }
    }
}
