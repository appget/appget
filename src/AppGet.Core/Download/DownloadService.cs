using System.Collections.Generic;
using System.Linq;
using AppGet.Core.Download;

namespace AppGet.Download
{
    public interface IDownloadService
    {
        void DownloadFile(string url, string fileName);
    }

    public class DownloadService : IDownloadService
    {
        private readonly IEnumerable<IDownloadClient> _downloadClients;

        public DownloadService(IEnumerable<IDownloadClient> downloadClients)
        {
            _downloadClients = downloadClients;
        }

        public void DownloadFile(string url, string fileName)
        {
            var client = _downloadClients.SingleOrDefault(c => c.CanHandleProtocol(url));

            if (client == null) return;

            client.DownloadFile(url, fileName);
        }
    }
}
