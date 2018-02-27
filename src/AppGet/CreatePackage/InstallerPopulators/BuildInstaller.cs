using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppGet.CommandLine.Prompts;
using AppGet.CreatePackage.Parsers;
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
        Task<Installer> Populate(string url);
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


        public async Task<Installer> Populate(string url)
        {
            var uri = new Uri(url, UriKind.Absolute);


            Installer installer = null;

            if (uri.Scheme == Uri.UriSchemeHttp)
            {
                _logger.Warn("Download link is using HTTP protocol. Checking HTTPS availability.");

                try
                {
                    installer = await DownloadInstaller(uri.ToHttps());
                    _logger.Info("Installer aprears to be avilable using HTTPS. Using HTTPS instead.");

                }
                catch (Exception e)
                {
                    _logger.Warn(e, "HTTPS switch over failed.");
                }
            }

            if (installer == null)
            {
                installer = await DownloadInstaller(uri);
            }

            foreach (var populater in _populaters)
            {
                populater.Populate(installer);
            }

            return installer;
        }

        private async Task<Installer> DownloadInstaller(Uri uri)
        {
            var installer = new Installer();
            var filePath = await _fileTransferService.TransferFile(uri.ToString(), _pathResolver.TempFolder, null);
            var sha256 = _sha256.CalculateHash(filePath);
            Console.WriteLine($"SHA-256: {sha256}");
            if (VersionParser.Parse(uri) != null)
            {
                installer.Sha256 = sha256;
            }
            else
            {
                _logger.Warn("Download URL doesn't aprear to point to a specific version. WILL NOT assign a SHA-256 to this installer.");
            }

            installer.Location = uri.ToString();
            installer.FilePath = filePath;
            return installer;
        }
    }
}