using System.Collections.Generic;
using System.Linq;
using NLog;

namespace AppGet.Download
{
    public interface IDownloadService
    {
        void DownloadFile(string url, string fileName);
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

        public void DownloadFile(string url, string fileName)
        {
            var client = _downloadClients.SingleOrDefault(c => c.CanHandleProtocol(url));

            if (client == null)
            {
                _logger.Trace("No download client found that could handle: {0}", url);
                
                throw new ProtocolNotSupportedException("No download client found that could handle: {0}", url);
            }

            client.DownloadFile(url, fileName);
        }
    }
}
