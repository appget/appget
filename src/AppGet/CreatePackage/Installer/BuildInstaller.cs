using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppGet.CreatePackage.Parsers;
using AppGet.Crypto.Hash.Algorithms;
using AppGet.Extensions;
using AppGet.FileTransfer;
using AppGet.HostSystem;
using NLog;

namespace AppGet.CreatePackage.Installer
{
    public interface IComposeInstaller
    {
        Task<InstallerBuilder> Compose(string url, bool interactive);
    }

    public class ComposeInstaller : IComposeInstaller
    {
        private readonly IFileTransferService _fileTransferService;
        private readonly IEnumerable<IInstallerPrompt> _prompts;
        private readonly IPathResolver _pathResolver;
        private readonly Logger _logger;
        private readonly Sha256Hash _sha256 = new Sha256Hash();

        public ComposeInstaller(IFileTransferService fileTransferService, IEnumerable<IInstallerPrompt> prompts, IPathResolver pathResolver, Logger logger)
        {
            _fileTransferService = fileTransferService;
            _prompts = prompts;
            _pathResolver = pathResolver;
            _logger = logger;
        }


        public async Task<InstallerBuilder> Compose(string url, bool interactive)
        {
            var uri = new Uri(url, UriKind.Absolute);


            InstallerBuilder installer = null;

            if (uri.Scheme == Uri.UriSchemeHttp)
            {
                _logger.Warn("Download link is using HTTP protocol. Checking HTTPS availability.");

                try
                {
                    installer = await DownloadInstaller(uri.ToHttps());
                    _logger.Info("Installer appears to be available using HTTPS. Using HTTPS instead.");

                }
                catch (Exception e)
                {
                    _logger.Warn(e, "HTTPS upgrade failed.");
                }
            }

            if (installer == null)
            {
                installer = await DownloadInstaller(uri);
            }

            if (interactive)
            {
                foreach (var prompt in _prompts.Where(p => p.ShouldPrompt(installer)))
                {
                    prompt.Invoke(installer);
                }
            }

            return installer;
        }

        private async Task<InstallerBuilder> DownloadInstaller(Uri uri)
        {
            var installer = new InstallerBuilder();
            var filePath = await _fileTransferService.TransferFile(uri.ToString(), _pathResolver.TempFolder, null);

            if (VersionParser.Parse(uri) != null)
            {
                var sha256 = _sha256.CalculateHash(filePath);
                _logger.Info($"SHA-256: {sha256}");
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