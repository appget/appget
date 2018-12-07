using AppGet.Crypto.Hash;
using AppGet.Extensions;
using AppGet.FileSystem;
using AppGet.HostSystem;
using NLog;

namespace AppGet.FileTransfer
{
    public interface ITransferCacheService
    {
        bool IsValid(string path, string sha256);
        string GetCacheFolder(string sha256);
        void Purge();
    }

    public class TransferCacheService : ITransferCacheService
    {
        private readonly IFileSystem _fileSystem;
        private readonly IChecksumService _checksumService;
        private readonly IPathResolver _pathResolver;
        private readonly Logger _logger;

        public TransferCacheService(IFileSystem fileSystem, IChecksumService checksumService, IPathResolver pathResolver, Logger logger)
        {
            _fileSystem = fileSystem;
            _checksumService = checksumService;
            _pathResolver = pathResolver;
            _logger = logger;
        }

        public bool IsValid(string path, string sha256)
        {
            if (string.IsNullOrWhiteSpace(sha256))
            {
                return false;
            }

            if (!_fileSystem.FileExists(path))
            {
                _logger.Debug("No existing download was found");
                return false;
            }

            try
            {
                _checksumService.ValidateHash(path, sha256);
                _logger.Debug($"Installer is already downloaded: {path}");

                return true;
            }
            catch (ChecksumVerificationException)
            {
                _logger.Warn("Checksum verification failed. ignoring cache.");
            }

            return false;
        }

        public string GetCacheFolder(string sha256)
        {
            return sha256.IsNullOrWhiteSpace() ? _pathResolver.TempFolder : _pathResolver.InstallerCacheFolder;
        }

        public void Purge()
        {
            _fileSystem.ClearDirectory(_pathResolver.InstallerCacheFolder, true);
            _fileSystem.ClearDirectory(_pathResolver.TempFolder, true);
        }
    }
}