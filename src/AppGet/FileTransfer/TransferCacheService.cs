using AppGet.Crypto.Hash;
using AppGet.FileSystem;
using AppGet.Manifests;
using NLog;

namespace AppGet.FileTransfer
{
    public interface ITransferCacheService
    {
        bool IsValid(string path, FileHash hash);
    }

    public class TransferCacheService : ITransferCacheService
    {
        private readonly IFileSystem _fileSystem;
        private readonly IChecksumService _checksumService;
        private readonly Logger _logger;

        public TransferCacheService(IFileSystem fileSystem, IChecksumService checksumService, Logger logger)
        {
            _fileSystem = fileSystem;
            _checksumService = checksumService;
            _logger = logger;
        }

        public bool IsValid(string path, FileHash hash)
        {
            if (hash == null || !_fileSystem.FileExists(path))
            {
                return false;
            }

            try
            {
                _checksumService.ValidateHash(path, hash);
                _logger.Debug($"Installer is already downloaded: {path}");
                return true;
            }
            catch (ChecksumVerificationException)
            {
                _logger.Warn("Checksum verification failed. ignoring cache.");
            }

            return false;
        }
    }
}
