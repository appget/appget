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
        Task Compose(InstallerBuilder installerBuilder, bool interactive);
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


        public async Task Compose(InstallerBuilder installerBuilder, bool interactive)
        {
            var uri = new Uri(installerBuilder.Location, UriKind.Absolute);

            if (uri.Scheme == Uri.UriSchemeHttp)
            {
                _logger.Warn("Download link is using HTTP protocol. Checking HTTPS availability.");

                try
                {
                    installerBuilder.Location = uri.ToHttps().ToString();
                    await DownloadInstaller(installerBuilder);
                    _logger.Info("Installer appears to be available using HTTPS. Using HTTPS instead.");

                }
                catch (Exception e)
                {
                    _logger.Warn(e, "HTTPS upgrade failed.");
                }
            }

            if (installerBuilder.FilePath == null)
            {
                installerBuilder.Location = uri.ToString();
                await DownloadInstaller(installerBuilder);
            }

            if (interactive)
            {
                foreach (var prompt in _prompts.Where(p => p.ShouldPrompt(installerBuilder)))
                {
                    prompt.Invoke(installerBuilder);
                }
            }
        }

        private async Task DownloadInstaller(InstallerBuilder installerBuilder)
        {
            var filePath = await _fileTransferService.TransferFile(installerBuilder.Location, _pathResolver.TempFolder, installerBuilder.FileVerificationInfo);

            if (VersionParser.Parse(new Uri(installerBuilder.Location)) != null)
            {
                var sha256 = _sha256.CalculateHash(filePath);
                _logger.Info($"SHA-256: {sha256}");
                installerBuilder.Sha256 = sha256;
            }
            else
            {
                _logger.Warn("Download URL doesn't aprear to point to a specific version. WILL NOT assign a SHA-256 to this installer.");
            }

            installerBuilder.FilePath = filePath;
        }
    }
}