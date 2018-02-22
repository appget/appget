using System;
using System.Collections.Generic;
using AppGet.CommandLine.Prompts;
using AppGet.Crypto.Hash.Algorithms;
using AppGet.Extensions;
using AppGet.FileTransfer;
using AppGet.HostSystem;
using AppGet.Manifests;
using NLog;

namespace AppGet.CreatePackage.InstallerPopulators
{
    public interface IBuildInstaller
    {
        Installer Populate();
        Installer Populate(string url);
    }

    public class BuildInstaller : IBuildInstaller
    {
        private readonly IFileTransferService _fileTransferService;
        private readonly IEnumerable<IPopulateInstaller> _populaters;
        private readonly IPathResolver _pathResolver;
        private readonly IUrlPrompt _urlPrompt;
        private readonly Logger _logger;
        private readonly Sha256Hash _sha256 = new Sha256Hash();

        public BuildInstaller(IFileTransferService fileTransferService, IEnumerable<IPopulateInstaller> populaters, IPathResolver pathResolver, IUrlPrompt urlPrompt, Logger logger)
        {
            _fileTransferService = fileTransferService;
            _populaters = populaters;
            _pathResolver = pathResolver;
            _urlPrompt = urlPrompt;
            _logger = logger;
        }


        public Installer Populate()
        {
            var url = _urlPrompt.Request("Installer direct download URL:", null);
            return Populate(url);
        }

        public Installer Populate(string url)
        {
            var uri = new Uri(url, UriKind.Absolute);


            Installer installer = null;

            if (uri.Scheme == Uri.UriSchemeHttp)
            {
                _logger.Warn("Download link is using HTTP protocol. Checking HTTPS availability.");

                try
                {
                    installer = DownloadInstaller(uri.ToHttps());
                    _logger.Info("Installer aprears to be avilable using HTTPS. Using HTTPS instead.");

                }
                catch (Exception e)
                {
                    _logger.Warn(e, "HTTPS switch over failed.");
                }
            }

            if (installer == null)
            {
                installer = DownloadInstaller(uri);
            }

            foreach (var populater in _populaters)
            {
                populater.Populate(installer);
            }

            return installer;
        }

        private Installer DownloadInstaller(Uri uri)
        {
            var installer = new Installer();
            var filePath = _fileTransferService.TransferFile(uri.ToString(), _pathResolver.TempFolder, null);
            installer.Sha256 = _sha256.CalculateHash(filePath);
            installer.Location = uri.ToString();
            installer.FilePath = filePath;
            return installer;
        }
    }
}