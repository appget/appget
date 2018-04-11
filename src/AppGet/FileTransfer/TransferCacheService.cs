using AppGet.Crypto.Hash;
using AppGet.FileSystem;
using AppGet.Manifests;
using NLog;

namespace AppGet.FileTransfer
{
    public interface ITransferCacheService
    {
        bool IsValid(string path, FileVerificationInfo verificationInfo);
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

        public bool IsValid(string path, FileVerificationInfo verificationInfo)
        {
            if (string.IsNullOrWhiteSpace(verificationInfo?.HashValue) || !_fileSystem.FileExists(path))
            {
                return false;
            }

            if (verificationInfo.FileSize > 0)
            {
                var size = _fileSystem.GetFileSize(path);
                if (size != verificationInfo.FileSize)
                {
                    _logger.Warn("File size miss-match. ignoring cache");

                    return false;
                }
            }

            try
            {
                _checksumService.ValidateHash(path, verificationInfo);
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