using System;
using AppGet.Crypto.Hash;
using AppGet.Crypto.Hash.Algorithms;
using AppGet.FileTransfer;
using AppGet.HostSystem;
using AppGet.Manifests;
using NLog;

namespace AppGet.CreatePackage
{
    public interface IXRayService
    {
        Installer Scan(string url);
    }

    public class XRayService : IXRayService
    {
        private readonly IFileTransferService _fileTransferService;
        private readonly IPathResolver _pathResolver;
        private readonly Logger _logger;
        private readonly Sha256Hash _sha256 = new Sha256Hash();

        public XRayService(IFileTransferService fileTransferService, IPathResolver pathResolver, Logger logger)
        {
            _fileTransferService = fileTransferService;
            _pathResolver = pathResolver;
            _logger = logger;
        }

        public Installer Scan(string url)
        {
            var installer = new Installer();

            var uri = new Uri(url, UriKind.Absolute);
            var filePath = _fileTransferService.TransferFile(uri.ToString(), _pathResolver.TempFolder, null);
            var sha256 = _sha256.CalculateHash(filePath);

            installer.Location = uri.ToString();
            installer.Sha256 = sha256;

            if (uri.Scheme == "http")
            {
                _logger.Warn("Download link is using HTTP protocol. Will now check if same file is available using HTTPS.");
                var httpsUri = new Uri(uri.ToString().Replace("http://", "https://"));

                filePath = _fileTransferService.TransferFile(httpsUri.ToString(), _pathResolver.TempFolder, null);
                var httpsSha256 = _sha256.CalculateHash(filePath);

                if (httpsSha256 == sha256)
                {
                    _logger.Info("File downloaded using HTTPS has same hash as HTTP, using the HTTPS url instead.");
                    installer.Location = httpsUri.ToString();
                }
            }

            return installer;
        }

    }
}
