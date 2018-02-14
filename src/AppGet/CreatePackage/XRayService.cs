using System;
using System.Diagnostics;
using AppGet.Crypto.Hash.Algorithms;
using AppGet.Extensions;
using AppGet.FileTransfer;
using AppGet.HostSystem;
using AppGet.Manifests;
using NLog;


namespace AppGet.CreatePackage
{
    public interface IXRayService
    {
        Installer Scan(string url, out FileVersionInfo fileVersionInfo);
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

        public Installer Scan(string url, out FileVersionInfo fileVersionInfo)
        {
            var installer = new Installer();

            var uri = new Uri(url, UriKind.Absolute);

            string filePath = null;

            if (uri.Scheme == Uri.UriSchemeHttp)
            {
                _logger.Warn("Download link is using HTTP protocol. Checking HTTPS availability.");

                try
                {
                    filePath = DownloadInstaller(uri.ToHttps(), installer);
                    _logger.Info("Installer aprears to be avilable using HTTPS. Using HTTPS instead.");
                }
                catch (Exception e)
                {
                    _logger.Warn(e, "HTTPS switch over failed.");
                }
            }

            if (filePath == null)
            {
                filePath = DownloadInstaller(uri, installer);
            }

            fileVersionInfo = FileVersionInfo.GetVersionInfo(filePath);

            return installer;
        }

        private string DownloadInstaller(Uri uri, Installer installer)
        {
            var filePath = _fileTransferService.TransferFile(uri.ToString(), _pathResolver.TempFolder, null);
            installer.Sha256 = _sha256.CalculateHash(filePath);
            installer.Location = uri.ToString();
            return filePath;
        }

    }
}
